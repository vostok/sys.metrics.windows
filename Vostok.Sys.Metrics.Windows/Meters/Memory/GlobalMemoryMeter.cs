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
                    (c, value) => c.Result.Kernel.PagedPool = DataSize.FromBytes(value))
                .WithCounter("Memory", "Pool Nonpaged Bytes",
                    (c, value) => c.Result.Kernel.NonpagedPool = DataSize.FromBytes(value))
                .WithCounter("Memory", "Standby Cache Normal Priority Bytes",
                    (c, value) => c.Result.Cache.Standby.NormalPriority = DataSize.FromBytes(value))
                .WithCounter("Memory", "Standby Cache Reserve Bytes",
                    (c, value) => c.Result.Cache.Standby.Reserve = DataSize.FromBytes(value))
                .WithCounter("Memory", "Committed Bytes",
                    (c, value) => c.Result.Commit.Committed = DataSize.FromBytes(value))
                .WithCounter("Memory", "Commit Limit",
                    (c, value) => c.Result.Commit.Limit = DataSize.FromBytes(value))
                .WithCounter("Memory", "System Cache Resident Bytes",
                    (c, value) => c.Result.Cache.File = DataSize.FromBytes(value))
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