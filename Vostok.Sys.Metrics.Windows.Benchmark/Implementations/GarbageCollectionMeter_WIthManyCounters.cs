using System;
using System.Diagnostics;
using Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters;
using Vostok.Sys.Metrics.Windows.Meters.DotNet;
using Vostok.Sys.Metrics.Windows.Native.Utilities;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

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
    public class GarbageCollectionMeter_WithManyCounters : IDisposable
    {
        public GarbageCollectionMeter_WithManyCounters(Process process = null)
            : this(process?.Id ?? ProcessUtility.CurrentProcessId)
        {}

        public GarbageCollectionMeter_WithManyCounters(int pid)
            : this(PerformanceCounterFactory_Old.Default,
                InstanceNameProviders.DotNet.ForPid(pid))
        {
        }

        internal GarbageCollectionMeter_WithManyCounters(IPerformanceCounterFactory_Old counterFactory, Func<string> instanceNameProvider)
        {
            gen0CollectionCounter = counterFactory.Create(".NET CLR Memory", "# Gen 0 Collections", instanceNameProvider);
            gen1CollectionCounter = counterFactory.Create(".NET CLR Memory", "# Gen 1 Collections", instanceNameProvider);
            gen2CollectionCounter = counterFactory.Create(".NET CLR Memory", "# Gen 2 Collections", instanceNameProvider);
            timeInCollectionCounter = counterFactory.Create(".NET CLR Memory", "% Time in GC", instanceNameProvider);
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
            var info = new GarbageCollectionInfo
            {
                Gen0CollectionsSinceStart = (long) gen0CollectionCounter.Observe(),
                Gen1CollectionsSinceStart = (long) gen1CollectionCounter.Observe(),
                Gen2CollectionsSinceStart = (long) gen2CollectionCounter.Observe(),
                TimeInGCPercent = timeInCollectionCounter.Observe()
            };
            info.Gen0CollectionsDelta = Math.Max(0, info.Gen0CollectionsSinceStart - previous.Gen0CollectionsSinceStart);
            info.Gen1CollectionsDelta = Math.Max(0, info.Gen1CollectionsSinceStart - previous.Gen1CollectionsSinceStart);
            info.Gen2CollectionsDelta = Math.Max(0, info.Gen2CollectionsSinceStart - previous.Gen2CollectionsSinceStart);
            previous = info;
            return info;
        }

        public void Dispose()
        {
            gen0CollectionCounter.Dispose();
            gen1CollectionCounter.Dispose();
            gen2CollectionCounter.Dispose();
            timeInCollectionCounter.Dispose();
        }

        private GarbageCollectionInfo previous;
        private readonly IPerformanceCounter<double> gen0CollectionCounter;
        private readonly IPerformanceCounter<double> gen1CollectionCounter;
        private readonly IPerformanceCounter<double> gen2CollectionCounter;
        private readonly IPerformanceCounter<double> timeInCollectionCounter;
    }
}