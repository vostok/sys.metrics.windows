using System;
using System.Collections.Generic;
using System.Linq;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch
{
    internal class ArrayPerformanceCounter<T> : IWildCardPerformanceCounter<T> where T : new ()
    {
        private readonly string instanceNameWildcard;
        private readonly Action<CounterContext<T>, string, int> setInstanceName;
        private readonly PdhCounterInfo<T>[] counters;
 
        private readonly Dictionary<string, CounterContext<T>> contexts = new Dictionary<string, CounterContext<T>>();

        private PdhQuery query;
        private readonly ResizeableBuffer resizeableBuffer = new ResizeableBuffer();
        private readonly InstancesCounter instancesCounter = new InstancesCounter();
        private readonly List<Sample> samples = new List<Sample>();
        private readonly List<KeyValuePair<string, CounterContext<T>>> sortBuffer = new List<KeyValuePair<string, CounterContext<T>>>();

        public ArrayPerformanceCounter(CounterInfo<T>[] counters, string instanceNameWildcard, Action<CounterContext<T>, string, int> setInstanceName)
        {
            this.instanceNameWildcard = instanceNameWildcard;
            this.setInstanceName = setInstanceName;
            this.counters = counters
                .Select(x => new PdhCounterInfo<T> {Info = x})
                .ToArray();
        }

        public T[] Observe()
        {
            if (query == null || query.IsInvalid)
                Init();

            Collect();

            contexts.Clear();
            
            for (var i = 0; i < counters.Length; ++i)
            {
                ref var counter = ref counters[i];
                ObtainSamples(counter.PdhCounter);
                instancesCounter.Clear();
                foreach (var sample in samples)
                {
                    if (!contexts.TryGetValue(sample.Instance, out var ctx))
                    {
                        ctx = new CounterContext<T>();
                        
                        contexts[sample.Instance] = ctx;
                        ctx.Result = Factory.Create<T>();
                        setInstanceName(ctx, sample.Instance, instancesCounter.GetAndIncrement(sample.Instance));
                    }

                    counter.Info.SetValue(ctx, sample.Value);
                }
            }

            sortBuffer.Clear();
            foreach (var kvp in contexts)
                sortBuffer.Add(kvp);
            sortBuffer.Sort((x, y) => string.Compare(x.Key, y.Key, StringComparison.Ordinal));

            var result = new T[contexts.Count];
            var index = 0;
            foreach (var kvp in sortBuffer)
                result[index++] = kvp.Value.Result;

            return result;
        }

        private struct Sample
        {
            public string Instance;
            public double Value;
        }

        private void Init()
        {
            query = PdhUtilities.OpenRealtimeQuery();
            for (var i = 0; i < counters.Length; ++i)
            {
                ref var counter = ref counters[i];
                var counterPath = CounterPathFactory.Create(counter.Info.Name.CategoryName, counter.Info.Name.CounterName, instanceNameWildcard);
                counter.PdhCounter = query.AddCounter(counterPath);
            }
        }

        private void Collect()
        {
            var status = query.CollectQueryData();
            if (status == PdhStatus.PDH_NO_DATA)
                return;
            status.EnsureSuccess(nameof(PdhUtilities.CollectQueryData));
        }

        private unsafe void ObtainSamples(PdhCounter counter)
        {
            samples.Clear();
            while (true)
            {
                var size = counter.EstimateFormattedCounterArraySize();
                var buffer = resizeableBuffer.Get(ref size);
                
                fixed (byte* bytePtr = buffer)
                {
                    var ptr = (PDH_FMT_COUNTERVALUE_ITEM*) bytePtr;
                    var status = counter.GetFormattedCounterArray(ref size, out var count, ptr);
                    if (status.IsLargerBufferRequired())
                        continue;
                    if (status == PdhStatus.PDH_INVALID_DATA || status == PdhStatus.PDH_CSTATUS_INVALID_DATA) // some counters require two raw values to calculate sample and may fail on first call
                        return;
                    status.EnsureSuccess(nameof(PdhUtilities.GetFormattedCounterArray));
                    
                    if (count*sizeof(PDH_FMT_COUNTERVALUE_ITEM) > buffer.Length)
                        throw new InvalidOperationException($"Buffer overflow check failed. ItemCount: {count}, ItemSize: {sizeof(PDH_FMT_COUNTERVALUE_ITEM)}, BufferSize: {buffer.Length}");
                    
                    for (var i = 0; i < count; ++i)
                        samples.Add(new Sample
                        {
                            Instance = new string(ptr[i].Name),
                            Value = ptr[i].FmtValue.DoubleValue
                        });
                    return;
                }
            }
        }

        public void Dispose()
            => query.Dispose();
    }
}