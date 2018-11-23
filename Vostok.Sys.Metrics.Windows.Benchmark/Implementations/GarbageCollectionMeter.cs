using System;
using Vostok.Sys.Metrics.PerfCounters;
using Vostok.Sys.Metrics.Windows.Meters.DotNet;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations
{
    /// <summary>
    /// Measures GC counts since process start and GC counts delta since last observation.
    /// <para>
    /// Internally uses .NET CLR Memory performance counters:
    /// </para>
    /// <list type="bullet">
    ///     <item>
    ///         # Gen 0 Collections
    ///     </item>
    ///     <item>
    ///         # Gen 1 Collections
    ///     </item>
    ///     <item>
    ///         # Gen 2 Collections
    ///     </item>
    ///     <item>
    ///         % Time in GC
    ///     </item>
    /// </list>
    /// </summary>
    internal class GarbageCollectionMeter : IDisposable
    {
        //TODO: remove performance counters and make lightweight only-for-current-process GG meter

        public GarbageCollectionMeter(IPerformanceCounterFactory counterFactory, Func<string> instanceNameProvider)
        {
            counter = counterFactory
                .Create<GarbageCollectionInfo>() // .Create<T>
                .AddCounter(".NET CLR Memory", "# Gen 0 Collections", (c, x) => c.Result.Gen0CollectionsSinceStart = (long) x)
                .AddCounter(".NET CLR Memory", "# Gen 1 Collections", (c, x) => c.Result.Gen1CollectionsSinceStart = (long) x)
                .AddCounter(".NET CLR Memory", "# Gen 2 Collections", (c, x) => c.Result.Gen2CollectionsSinceStart = (long) x)
                .AddCounter(".NET CLR Memory", "% Time in GC", (c, x) => c.Result.TimeInGCPercent = x)
                .Build(instanceNameProvider);
        }

        /// <summary>
        /// Returns info about GC for the process specified in the constructor.
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
        public GarbageCollectionInfo GetGarbageCollectionInfo()
        {
            var value = counter.Observe();
            value.Gen0CollectionsDelta = Math.Max(0, value.Gen0CollectionsSinceStart - previous.Gen0CollectionsSinceStart);
            value.Gen1CollectionsDelta = Math.Max(0, value.Gen1CollectionsSinceStart - previous.Gen1CollectionsSinceStart);
            value.Gen2CollectionsDelta = Math.Max(0, value.Gen2CollectionsSinceStart - previous.Gen2CollectionsSinceStart);
            previous = value;
            return value;
        }

        public void Dispose()
        {
            counter.Dispose();
        }

        private readonly IPerformanceCounter<GarbageCollectionInfo> counter;
        private GarbageCollectionInfo previous = new GarbageCollectionInfo();
    }
}