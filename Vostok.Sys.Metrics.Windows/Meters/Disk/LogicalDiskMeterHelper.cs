using System;
using Vostok.Sys.Metrics.PerfCounters;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
{
    internal static class LogicalDiskMeterHelper
    {
        public static IPerformanceCounterBuilder<DiskMetrics> CreateCounters(IPerformanceCounterFactory counterFactory)
            => counterFactory.Create<DiskMetrics>()
                .AddCounter(Category.LogicalDisk, "Current Disk Queue Length",
                    (c, v) => c.Result.QueueLength = (int) v)
                .AddCounter(Category.LogicalDisk, "Disk Read Bytes/sec",
                    (c, v) => c.Result.ReadBytesPerSecond = (long) v)
                .AddCounter(Category.LogicalDisk, "Disk Write Bytes/sec",
                    (c, v) => c.Result.WriteBytesPerSecond = (long) v)
                .AddCounter(Category.LogicalDisk, "Disk Reads/sec",
                    (c, v) => c.Result.ReadsPerSecond = (int) v)
                .AddCounter(Category.LogicalDisk, "Disk Writes/sec",
                    (c, v) => c.Result.WritesPerSecond = (int) v)
                .AddCounter(Category.LogicalDisk, "Avg. Disk sec/Read",
                    (c, v) => c.Result.ReadLatency = TimeSpan.FromSeconds(v))
                .AddCounter(Category.LogicalDisk, "Avg. Disk sec/Write",
                    (c, v) => c.Result.WriteLatency = TimeSpan.FromSeconds(v))
                .AddCounter(Category.LogicalDisk, "Free Megabytes",
                    (c, v) => c.Result.FreeSpaceBytes = (long) v)
                .AddCounter(Category.LogicalDisk, "% Idle Time",
                    (c, v) => c.Result.Load = Math.Max(0, (100 - v) / 100));
    }
}