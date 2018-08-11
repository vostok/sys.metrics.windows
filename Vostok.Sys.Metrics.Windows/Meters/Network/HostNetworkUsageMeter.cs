using System;
using System.Linq;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Utilities;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    public class HostNetworkUsageMeter : IDisposable
    {
        private readonly IPerformanceCounter<InterfaceUsageMetrics[]> counter;

        public HostNetworkUsageMeter()
            : this(PerformanceCounterFactory.Default)
        {
        }

        private HostNetworkUsageMeter(IPerformanceCounterFactory counterFactory)
        {
            counter = counterFactory.Create<InterfaceUsageMetrics>()
                .WithCounter("Network Interface", "Bytes Received/sec",
                    (c, v) => c.Result.ReceivedPerSecond = DataSize.FromBytes(v))
                .WithCounter("Network Interface", "Bytes Sent/sec",
                    (c, v) => c.Result.SentPerSecond = DataSize.FromBytes(v))
                .WithCounter("Network Interface", "Current Bandwidth",
                    (c, v) => c.Result.Bandwidth = DataSize.FromBytes(v/8))
                .BuildWildcard("*", (context, s) => context.Result.Interface = s);
        }

        public NetworkUsageMetrics GetNetworkMetrics()
        {
            var upInterfaces = ActiveNetworkInterfaceCache.Instance.GetUpInterfaces();
            
            return new NetworkUsageMetrics(
                counter
                    .Observe()
                    .Where(x => upInterfaces.Contains(ActiveNetworkInterfaceCache.GetAdapterIdentity(x.Interface))).ToArray());
        }

        public void Dispose()
            => counter.Dispose();
    }
}