using System;
using System.Collections.Generic;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters
{
    internal unsafe class InstanceNameUtility
    {
        private readonly string counterPath;
        private readonly object sync = new object();
        private readonly Dictionary<int, CachedInstanceName> instanceNameCache = new Dictionary<int, CachedInstanceName>();
        private readonly List<int> valuesToRemove = new List<int>();
        private readonly InstancesCounter instancesCounter = new InstancesCounter();
        private readonly ResizeableBuffer resizeableBuffer = new ResizeableBuffer();
        
        private PdhQuery query;
        private PdhCounter counter;

        public static readonly InstanceNameUtility AllProcesses = new InstanceNameUtility(@"\Process(*)\ID Process");
        public static readonly InstanceNameUtility NetProcesses = new InstanceNameUtility(@"\.NET CLR Memory(*)\Process ID");

        private InstanceNameUtility(string counterPath)
            => this.counterPath = counterPath;

        public Dictionary<int, string> ObtainInstanceNames()
        {
            if (query == null || query.IsInvalid)
                Init();

            query.CollectQueryData().EnsureSuccess(nameof(Pdh.PdhCollectQueryData));

            lock (sync)
            {
                while (true)
                {
                    var bufferSize = counter.EstimateRawCounterArraySize();

                    var buffer = resizeableBuffer.Get(ref bufferSize);

                    fixed (byte* bytePtr = buffer)
                    {
                        var ptr = (PDH_RAW_COUNTER_ITEM*) bytePtr;
                        var status = counter.GetRawCounterArray(ref bufferSize, out var itemCount, ptr);

                        if (status.IsLargerBufferRequired())
                            continue;

                        status.EnsureSuccess(nameof(Pdh.PdhGetRawCounterArray));
                        
                        if (itemCount*sizeof(PDH_RAW_COUNTER_ITEM) > buffer.Length)
                            throw new InvalidOperationException($"Buffer overflow check failed. ItemCount: {itemCount}, ItemSize: {sizeof(PDH_RAW_COUNTER_ITEM)}, BufferSize: {buffer.Length}");

                        return BuildProcessIdToInstanceNameMap(itemCount, ptr);
                    }
                }
            }
        }

        private void Init()
        {
            query = PdhUtilities.OpenRealtimeQuery();
            counter = query.AddCounter(counterPath);
        }

        private Dictionary<int, string> BuildProcessIdToInstanceNameMap(int itemCount, PDH_RAW_COUNTER_ITEM* ptr)
        {
            var map = new Dictionary<int, string>(itemCount);

            instancesCounter.Clear();
            for (var i = 0; i < itemCount; i++)
            {
                var currentPid = (int) ptr[i].RawValue.FirstValue;
                var currentName = new string(ptr[i].Name);

                var instanceIndex = instancesCounter.GetAndIncrement(currentName);

                map[currentPid] = GetInstanceName(currentName, currentPid, instanceIndex);
            }
            
            CleanupCache(map);

            return map;
        }

        private void CleanupCache(Dictionary<int, string> newMap)
        {
            valuesToRemove.Clear();
            foreach (var pid in instanceNameCache.Keys)
                if (!newMap.ContainsKey(pid))
                    valuesToRemove.Add(pid);
            foreach (var pid in valuesToRemove)
                instanceNameCache.Remove(pid);
        }

        private string GetInstanceName(string currentName, int pid, int instanceIndex)
        {
            // Use InstanceName strings cache to reduce memory usage. InstanceName can be reused if pid, instance index and process name haven't changed
            if (instanceNameCache.TryGetValue(pid, out var val) && val.InstanceIndex == instanceIndex && currentName == val.ProcessName)
                return val.InstanceName;
            var value = new CachedInstanceName
            {
                InstanceName = instanceIndex == 0 ? currentName : currentName + '#' + instanceIndex,
                InstanceIndex = instanceIndex,
                ProcessName = currentName
            };
            instanceNameCache[pid] = value;
            return value.InstanceName;
        }
        

        private struct CachedInstanceName
        {
            public string InstanceName;
            public string ProcessName;
            public int InstanceIndex;
        }
    }
}