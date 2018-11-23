using System;
using System.IO;
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
    public class LogicalDiskMeter : IDisposable
    {
        private readonly IPerformanceCounter<DiskMetrics> counter;
        private readonly string driveName;
        private bool initialized;

        public LogicalDiskMeter(char driveLetter)
            : this(driveLetter, PerformanceCounterFactory.Default)
        { }

        private LogicalDiskMeter(char? driveLetter, IPerformanceCounterFactory counterFactory)
        {
            var instanceName = driveLetter.HasValue ? driveLetter.ToString() + Path.VolumeSeparatorChar : "_Total";
            driveName = driveLetter.HasValue ? instanceName : "Total";

            counter = LogicalDiskMeterHelper
                .CreateCounters(counterFactory)
                .Build(instanceName);
        }

        public DiskMetrics GetDiskMetrics()
        {
            var metrics = GetDiskMetricsInternal();
            if (!initialized)
            {
                metrics.Load = 0;
                initialized = true;
            }

            metrics.Drive = driveName;

            return metrics;
        }

        private DiskMetrics GetDiskMetricsInternal() => counter.Observe();

        public void Dispose() => counter.Dispose();
    }
}