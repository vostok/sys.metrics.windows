using System;
using System.Linq;
using Vostok.Sys.Metrics.PerfCounters;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    public class HostNetworkUsageMeter : IDisposable
    {
        private readonly IPerformanceCounter<Observation<InterfaceUsageMetrics>[]> counter;

        public HostNetworkUsageMeter()
            : this(PerformanceCounterFactory.Default)
        {
        }

        private HostNetworkUsageMeter(IPerformanceCounterFactory counterFactory)
        {
            counter = counterFactory.Create<InterfaceUsageMetrics>()
                .AddCounter("Network Interface", "Bytes Received/sec",
                    (c, v) => c.Result.ReceivedBytesPerSecond = (long) v)
                .AddCounter("Network Interface", "Bytes Sent/sec",
                    (c, v) => c.Result.SentBytesPerSecond = (long) v)
                .AddCounter("Network Interface", "Current Bandwidth",
                    (c, v) => c.Result.BandwidthBytes = (long) (v/8))
                .BuildForMultipleInstances("*");
        }

        public NetworkUsageMetrics GetNetworkMetrics()
        {
            var upInterfaces = ActiveNetworkInterfaceCache.Instance.GetUpInterfaces();
            
            return new NetworkUsageMetrics(
                counter
                    .Observe()
                    .Where(x => upInterfaces.Contains(ActiveNetworkInterfaceCache.GetAdapterIdentity(x.Instance)))
                    .Select(
                        x =>
                        {
                            var val = x.Value;
                            val.Interface = x.Instance;
                            return val;
                        })
                    .ToArray());
        }

        public void Dispose()
            => counter.Dispose();
    }
}