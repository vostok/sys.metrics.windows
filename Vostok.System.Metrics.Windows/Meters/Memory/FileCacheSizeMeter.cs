using System;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.PerformanceCounters;
using Vostok.System.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.System.Metrics.Windows.Meters.Memory
{
    /// <summary>
    /// Measures current file cache size.
    /// 
    /// Internally uses "Memory.System Cache Resident Bytes" performance counter
    /// </summary>
    public class FileCacheSizeMeter : IDisposable
    {
        public FileCacheSizeMeter() : this(PerformanceCounterFactory.Default)
        {
        }

        private FileCacheSizeMeter(IPerformanceCounterFactory counterFactory)
        {
            counter = counterFactory.CreateCounter("Memory", "System Cache Resident Bytes", string.Empty);
        }

        /// <summary>
        /// Measures current file cache size.
        /// </summary>
        public DataSize GetCacheSize()
        {
            return DataSize.FromBytes((long) counter.Observe());
        }

        public void Dispose()
        {
            counter.Dispose();
        }

        private readonly IPerformanceCounter<double> counter;
    }
}