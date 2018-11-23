using System;
using Vostok.Sys.Metrics.PerfCounters;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
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
        private readonly IPerformanceCounter<Observation<DiskMetrics>[]> counter;

        public AllLogicalDisksMeter()
            : this(PerformanceCounterFactory.Default)
        { }

        private AllLogicalDisksMeter(IPerformanceCounterFactory counterFactory)
        {
            counter = LogicalDiskMeterHelper
                .CreateCounters(counterFactory)
                .BuildForMultipleInstances("*:");
        }

        public DisksMetrics GetDiskMetrics()
        {
            var metrics = GetDiskMetricsInternal();

            return new DisksMetrics(metrics);
        }

        private DiskMetrics[] GetDiskMetricsInternal()
        {
            var observations = counter.Observe();
            var data = new DiskMetrics[observations.Length];
            for (var i = 0; i < observations.Length; i++)
            {
                data[i] = observations[i].Value;
                data[i].Drive = observations[i].Instance;
            }

            return data;
        }

        public void Dispose() => counter.Dispose();
    }
}