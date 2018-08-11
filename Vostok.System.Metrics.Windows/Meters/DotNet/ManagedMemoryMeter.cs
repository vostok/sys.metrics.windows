using System;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.Native.Utilities;
using Vostok.System.Metrics.Windows.PerformanceCounters;
using Vostok.System.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.System.Metrics.Windows.Meters.DotNet
{
    /// <summary>
    /// Measures:
    /// <list type="bullet">
    ///     <item>GC count by generation since process start</item>
    ///     <item>GC count by generation since last observation</item>
    ///     <item>Heap generation sizes</item>
    ///     <item>Allocation rate per second</item>
    /// </list>
    /// <para>
    /// Internally uses .NET CLR Memory performance counters:
    /// </para>
    /// <list type="bullet">
    ///     <item># Gen 0 Collections</item>
    ///     <item># Gen 1 Collections</item>
    ///     <item># Gen 2 Collections</item>
    ///     <item>% Time in GC</item>
    ///     <item>Gen 0 heap size</item>
    ///     <item>Gen 1 heap size</item>
    ///     <item>Gen 2 heap size</item>
    ///     <item>Large Object Heap size</item>
    ///     <item># Bytes in all Heaps</item>
    ///     <item>Allocated Bytes/sec</item>
    /// 
    /// </list>
    /// <see href="https://blogs.msdn.microsoft.com/maoni/2004/06/03/gc-performance-counters/"/>
    /// </summary>    
    public class ManagedMemoryMeter : IDisposable
    {
        private GarbageCollectionInfo previous = new GarbageCollectionInfo();
        private readonly IPerformanceCounter<ManagedMemoryInfo> counter;
        private readonly int pid;

        internal ManagedMemoryMeter(IPerformanceCounterFactory counterFactory, int pid)
        {
            this.pid = pid;
            counter = CreateCounter(counterFactory, pid);
        }

        public ManagedMemoryMeter(int pid) : this(PerformanceCounterFactory.Default, pid)
        {
        }

        public ManagedMemoryMeter() : this(ProcessUtility.CurrentProcessId)
        {
        }

        /// <summary>
        /// Returns info about GC and heap for the process specified in the constructor.
        /// <para>
        /// Gen1 collection also triggers gen0, and gen2 collection triggers both gen1 and gen0.
        /// </para>
        /// <para>
        /// That's why after gen2 collection all 
        /// <see cref="GarbageCollectionInfo.Gen0CollectionsSinceStart"/>,
        /// <see cref="GarbageCollectionInfo.Gen1CollectionsSinceStart"/> and
        /// <see cref="GarbageCollectionInfo.Gen2CollectionsSinceStart"/>
        /// will increment.
        /// </para>
        /// </summary>
        public ManagedMemoryInfo GetManagedMemoryInfo()
        {
            var value = counter.Observe();

            if (pid == ProcessUtility.CurrentProcessId)
            {
                value.GC.Gen0CollectionsSinceStart = GC.CollectionCount(0);
                value.GC.Gen1CollectionsSinceStart = GC.CollectionCount(1);
                value.GC.Gen2CollectionsSinceStart = GC.CollectionCount(2);
            }
            
            value.GC.Gen0CollectionsDelta = Math.Max(0, value.GC.Gen0CollectionsSinceStart - previous.Gen0CollectionsSinceStart);
            value.GC.Gen1CollectionsDelta = Math.Max(0, value.GC.Gen1CollectionsSinceStart - previous.Gen1CollectionsSinceStart);
            value.GC.Gen2CollectionsDelta = Math.Max(0, value.GC.Gen2CollectionsSinceStart - previous.Gen2CollectionsSinceStart);
            previous = value.GC;
            return value;
        }

        private static IPerformanceCounter<ManagedMemoryInfo> CreateCounter(IPerformanceCounterFactory counterFactory, int pid)
        {
            var builder = counterFactory
                .Create<ManagedMemoryInfo>() // .Create<T>
                .WithCounter(Category.ClrMemory, "% Time in GC", (c, x) => c.Result.GC.TimeInGCPercent = x)
                .WithCounter(Category.ClrMemory, "Gen 0 heap size", (c, x) => c.Result.Heap.Gen0Size = DataSize.FromBytes(x))
                .WithCounter(Category.ClrMemory, "Gen 1 heap size", (c, x) => c.Result.Heap.Gen1Size = DataSize.FromBytes(x))
                .WithCounter(Category.ClrMemory, "Gen 2 heap size", (c, x) => c.Result.Heap.Gen2Size = DataSize.FromBytes(x))
                .WithCounter(Category.ClrMemory, "Large Object Heap size", (c, x) => c.Result.Heap.LargeObjectHeapSize = DataSize.FromBytes(x))
                .WithCounter(Category.ClrMemory, "# Bytes in all Heaps", (c, x) => c.Result.Heap.TotalSize = DataSize.FromBytes(x))
                .WithCounter(Category.ClrMemory, "Allocated Bytes/sec", (c, x) => c.Result.Heap.AllocationRate = DataSize.FromBytes(x));
            
            if (pid != ProcessUtility.CurrentProcessId)
                builder = builder
                    .WithCounter(Category.ClrMemory, "# Gen 0 Collections", (c, x) => c.Result.GC.Gen0CollectionsSinceStart = (long) x)
                    .WithCounter(Category.ClrMemory, "# Gen 1 Collections", (c, x) => c.Result.GC.Gen1CollectionsSinceStart = (long) x)
                    .WithCounter(Category.ClrMemory, "# Gen 2 Collections", (c, x) => c.Result.GC.Gen2CollectionsSinceStart = (long) x);
            
            return builder.Build(InstanceNameProviders.DotNet.ForPid(pid));
        }

        public void Dispose()
        {
            counter?.Dispose();
        }
    }
}