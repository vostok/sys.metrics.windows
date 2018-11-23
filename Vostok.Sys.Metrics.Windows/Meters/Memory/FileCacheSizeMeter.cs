using System;
using Vostok.Sys.Metrics.PerfCounters;

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
        public long GetFileCacheSizeBytes()
        {
            return (long) counter.Observe();
        }

        public void Dispose()
        {
            counter.Dispose();
        }

        private readonly IPerformanceCounter<double> counter;
    }
}