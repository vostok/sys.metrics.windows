using System;
using System.IO;
using Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Meters.Disk;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations
{
    
    /// <summary>
    /// Measures disk metrics. 
    /// <para>
    /// Internally uses LogicalDisk [driveLetter]: performance counters
    /// </para>
    /// <see href="https://blogs.technet.microsoft.com/askcore/2012/03/16/windows-performance-monitor-disk-counters-explained/">Windows Performance Monitor Disk Counters Explained</see>
    /// </summary>
    public class LogicalDiskMeter_Old : IDisposable
    {
        private readonly IPerformanceCounter<double> queueLengthCounter;
        private readonly IPerformanceCounter<double> readBytesPerSecondCounter;
        private readonly IPerformanceCounter<double> writeBytesPerSecondCounter;
        private readonly IPerformanceCounter<double> readsPerSecondCounter;
        private readonly IPerformanceCounter<double> writesPerSecondCounter;
        private readonly IPerformanceCounter<double> readLatencyCounter;
        private readonly IPerformanceCounter<double> writeLatencyCounter;
        private readonly IPerformanceCounter<double> idleTimeCounter;
        private readonly string driveName;
        private bool initialized;

        public LogicalDiskMeter_Old(char? driveLetter=null)
            : this(driveLetter, PerformanceCounterFactory_Old.Default)
        { }

        internal LogicalDiskMeter_Old(char? driveLetter, IPerformanceCounterFactory_Old counterFactory)
        {
            var instanceName = driveLetter.HasValue ? driveLetter.ToString() + Path.VolumeSeparatorChar : "_Total";
            driveName = driveLetter.HasValue ? instanceName : "Total";
            
            queueLengthCounter = counterFactory.Create(
                "LogicalDisk",
                "Current Disk Queue Length",
                instanceName);
            readBytesPerSecondCounter = counterFactory.Create(
                "LogicalDisk",
                "Disk Read Bytes/sec",
                instanceName);
            writeBytesPerSecondCounter = counterFactory.Create(
                "LogicalDisk",
                "Disk Write Bytes/sec",
                instanceName);
            readLatencyCounter = counterFactory.Create(
                "LogicalDisk",
                "Avg. Disk sec/Read",
                instanceName);
            writeLatencyCounter = counterFactory.Create(
                "LogicalDisk",
                "Avg. Disk sec/Write",
                instanceName);
            idleTimeCounter = counterFactory.Create(
                "LogicalDisk",
                "% Idle Time",
                instanceName);
            readsPerSecondCounter = counterFactory.Create(
                "LogicalDisk",
                "Disk Reads/sec",
                instanceName);
            writesPerSecondCounter = counterFactory.Create(
                "LogicalDisk",
                "Disk Writes/sec",
                instanceName);
        }

        public DiskMetrics GetDiskMetrics()
        {
            var metrics = GetDiskMetricsInternal();
            if (!initialized)
            {
                metrics.Load = 0;
                initialized = true;
            }

            return metrics;
        }
        
        private DiskMetrics GetDiskMetricsInternal()
        {
            return new DiskMetrics
            {
                QueueLength = (int) queueLengthCounter.Observe(),
                Load = Math.Max(0, (100 - idleTimeCounter.Observe()) / 100),
                ReadBytesPerSecond = (long) readBytesPerSecondCounter.Observe(),
                WriteBytesPerSecond = (long) writeBytesPerSecondCounter.Observe(),
                ReadLatency = TimeSpan.FromSeconds(readLatencyCounter.Observe()),
                WriteLatency = TimeSpan.FromSeconds(writeLatencyCounter.Observe()),
                ReadsPerSecond = (int) readsPerSecondCounter.Observe(),
                WritesPerSecond = (int) writesPerSecondCounter.Observe(),
                Drive = driveName
            };
        }

        public void Dispose()
        {
            queueLengthCounter.Dispose();
            idleTimeCounter.Dispose();
            queueLengthCounter.Dispose();
            readBytesPerSecondCounter.Dispose();
            readLatencyCounter.Dispose();
            writeBytesPerSecondCounter.Dispose();
            writeLatencyCounter.Dispose();
        }
    }
}