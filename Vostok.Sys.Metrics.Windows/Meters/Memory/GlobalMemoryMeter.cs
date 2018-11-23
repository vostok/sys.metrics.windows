using System;
using Vostok.Sys.Metrics.PerfCounters;

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
                .AddCounter("Memory", "Pool Paged Bytes",
                    (c, value) => c.Result.Kernel.PagedPoolBytes = (long) value)
                .AddCounter("Memory", "Pool Nonpaged Bytes",
                    (c, value) => c.Result.Kernel.NonpagedPoolBytes = (long) value)
                .AddCounter("Memory", "Standby Cache Normal Priority Bytes",
                    (c, value) => c.Result.Cache.Standby.NormalPriorityBytes = (long) value)
                .AddCounter("Memory", "Standby Cache Reserve Bytes",
                    (c, value) => c.Result.Cache.Standby.ReserveBytes = (long) value)
                .AddCounter("Memory", "Committed Bytes",
                    (c, value) => c.Result.Commit.CommittedBytes = (long) value)
                .AddCounter("Memory", "Commit Limit",
                    (c, value) => c.Result.Commit.LimitBytes = (long) value)
                .AddCounter("Memory", "System Cache Resident Bytes",
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