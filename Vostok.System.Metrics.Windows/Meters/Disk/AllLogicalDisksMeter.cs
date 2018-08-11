using System;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.PerformanceCounters;
using Vostok.System.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.System.Metrics.Windows.Meters.Disk
{
    /// <summary>
    /// Measures disk metrics. 
    /// <para>
    /// Internally uses LogicalDisk [driveLetter]: performance counters
    /// </para>
    /// <see href="https://blogs.technet.microsoft.com/askcore/2012/03/16/windows-performance-monitor-disk-counters-explained/">Windows Performance Monitor Disk Counters Explained</see>
    /// </summary>
    public class AllLogicalDisksMeter : IDisposable
    {
        private readonly IPerformanceCounter<DiskMetrics[]> counter;

        public AllLogicalDisksMeter()
            : this(PerformanceCounterFactory.Default)
        { }

        private AllLogicalDisksMeter(IPerformanceCounterFactory counterFactory)
        {
            counter = LogicalDiskMeterHelper
                .CreateCounters(counterFactory)
                .BuildWildcard("*:", (c, s) => c.Result.Drive = s);
        }

        public DisksMetrics GetDiskMetrics()
        {
            var metrics = GetDiskMetricsInternal();

            return new DisksMetrics(metrics);
        }

        private DiskMetrics[] GetDiskMetricsInternal() => counter.Observe();

        public void Dispose() => counter.Dispose();
    }
}