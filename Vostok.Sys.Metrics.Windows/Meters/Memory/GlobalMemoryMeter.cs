using System;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public class GlobalMemoryMeter : IDisposable
    {
        private IPerformanceCounter<GlobalMemoryInfo> counter;

        public GlobalMemoryMeter()
            :this(PerformanceCounterFactory.Default)
        {
        }

        public GlobalMemoryMeter(IPerformanceCounterFactory counterFactory)
        {
            counter = counterFactory.Create<GlobalMemoryInfo>()
                .WithCounter("Memory", "Pool Paged Bytes",
                    (c, value) => c.Result.Kernel.PagedPoolBytes = (long) value)
                .WithCounter("Memory", "Pool Nonpaged Bytes",
                    (c, value) => c.Result.Kernel.NonpagedPoolBytes = (long) value)
                .WithCounter("Memory", "Standby Cache Normal Priority Bytes",
                    (c, value) => c.Result.Cache.Standby.NormalPriorityBytes = (long) value)
                .WithCounter("Memory", "Standby Cache Reserve Bytes",
                    (c, value) => c.Result.Cache.Standby.ReserveBytes = (long) value)
                .WithCounter("Memory", "Committed Bytes",
                    (c, value) => c.Result.Commit.CommittedBytes = (long) value)
                .WithCounter("Memory", "Commit Limit",
                    (c, value) => c.Result.Commit.LimitBytes = (long) value)
                .WithCounter("Memory", "System Cache Resident Bytes",
                    (c, value) => c.Result.Cache.FileCacheBytes = (long) value)
                .Build();
        }

        public GlobalMemoryInfo GetGlobalMemoryInfo()
        {
            return counter.Observe();
        }
        
        public void Dispose()
        {
            counter.Dispose();
        }
    }
}