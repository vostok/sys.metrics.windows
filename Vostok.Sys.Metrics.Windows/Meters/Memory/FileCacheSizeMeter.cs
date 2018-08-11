using System;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
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