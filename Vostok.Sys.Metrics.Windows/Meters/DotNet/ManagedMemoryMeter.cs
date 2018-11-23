using System;
using Vostok.Sys.Metrics.PerfCounters;
using Vostok.Sys.Metrics.PerfCounters.InstanceNames;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.DotNet
{
    /// <summary>
    /// Measures:
    /// <list type="bullet">
    ///     <item><description>GC count by generation since process start</description></item>
    ///     <item><description>GC count by generation since last observation</description></item>
    ///     <item><description>Heap generation sizes</description></item>
    ///     <item><description>Allocation rate per second</description></item>
    /// </list>
    /// <para>
    /// Internally uses .NET CLR Memory performance counters:
    /// </para>
    /// <list type="bullet">
    ///     <item><description># Gen 0 Collections</description></item>
    ///     <item><description># Gen 1 Collections</description></item>
    ///     <item><description># Gen 2 Collections</description></item>
    ///     <item><description>% Time in GC</description></item>
    ///     <item><description>Gen 0 heap size</description></item>
    ///     <item><description>Gen 1 heap size</description></item>
    ///     <item><description>Gen 2 heap size</description></item>
    ///     <item><description>Large Object Heap size</description></item>
    ///     <item><description># Bytes in all Heaps</description></item>
    ///     <item><description>Allocated Bytes/sec</description></item>
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
                .AddCounter(Category.ClrMemory, "% Time in GC", (c, x) => c.Result.GC.TimeInGCPercent = x)
                .AddCounter(Category.ClrMemory, "Gen 0 heap size", (c, x) => c.Result.Heap.Gen0SizeBytes = (long) x)
                .AddCounter(Category.ClrMemory, "Gen 1 heap size", (c, x) => c.Result.Heap.Gen1SizeBytes = (long) x)
                .AddCounter(Category.ClrMemory, "Gen 2 heap size", (c, x) => c.Result.Heap.Gen2SizeBytes = (long) x)
                .AddCounter(Category.ClrMemory, "Large Object Heap size", (c, x) => c.Result.Heap.LargeObjectHeapSizeBytes = (long) x)
                .AddCounter(Category.ClrMemory, "# Bytes in all Heaps", (c, x) => c.Result.Heap.TotalSizeBytes = (long) x)
                .AddCounter(Category.ClrMemory, "Allocated Bytes/sec", (c, x) => c.Result.Heap.AllocatedBytesPerSecond = (long) x);
            
            if (pid != ProcessUtility.CurrentProcessId)
                builder = builder
                    .AddCounter(Category.ClrMemory, "# Gen 0 Collections", (c, x) => c.Result.GC.Gen0CollectionsSinceStart = (long) x)
                    .AddCounter(Category.ClrMemory, "# Gen 1 Collections", (c, x) => c.Result.GC.Gen1CollectionsSinceStart = (long) x)
                    .AddCounter(Category.ClrMemory, "# Gen 2 Collections", (c, x) => c.Result.GC.Gen2CollectionsSinceStart = (long) x);
            
            return builder.Build(InstanceNameProviders.DotNet.ForPid(pid));
        }

        public void Dispose()
        {
            counter?.Dispose();
        }
    }
}