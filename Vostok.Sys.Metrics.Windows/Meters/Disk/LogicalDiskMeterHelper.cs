using System;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
{
    internal static class LogicalDiskMeterHelper
    {
        public static IPerformanceCounterBuilder<DiskMetrics> CreateCounters(IPerformanceCounterFactory counterFactory)
            => counterFactory.Create<DiskMetrics>()
                .WithCounter(Category.LogicalDisk, "Current Disk Queue Length",
                    (c, v) => c.Result.QueueLength = (int) v)
                .WithCounter(Category.LogicalDisk, "Disk Read Bytes/sec",
                    (c, v) => c.Result.ReadBytesPerSecond = (long) v)
                .WithCounter(Category.LogicalDisk, "Disk Write Bytes/sec",
                    (c, v) => c.Result.WriteBytesPerSecond = (long) v)
                .WithCounter(Category.LogicalDisk, "Disk Reads/sec",
                    (c, v) => c.Result.ReadsPerSecond = (int) v)
                .WithCounter(Category.LogicalDisk, "Disk Writes/sec",
                    (c, v) => c.Result.WritesPerSecond = (int) v)
                .WithCounter(Category.LogicalDisk, "Avg. Disk sec/Read",
                    (c, v) => c.Result.ReadLatency = TimeSpan.FromSeconds(v))
                .WithCounter(Category.LogicalDisk, "Avg. Disk sec/Write",
                    (c, v) => c.Result.WriteLatency = TimeSpan.FromSeconds(v))
                .WithCounter(Category.LogicalDisk, "Free Megabytes",
                    (c, v) => c.Result.FreeSpaceBytes = (long) v)
                .WithCounter(Category.LogicalDisk, "% Idle Time",
                    (c, v) => c.Result.Load = Math.Max(0, (100 - v) / 100));
    }
}