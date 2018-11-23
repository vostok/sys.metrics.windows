using System;
using System.Linq;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    public struct NetworkUsageMetrics
    {
        public NetworkUsageMetrics(InterfaceUsageMetrics[] interfaceMetrics)
            => InterfaceMetrics = interfaceMetrics;

        public InterfaceUsageMetrics[] InterfaceMetrics { get; }

        public long ReceivedBytesPerSecond => InterfaceMetrics.Sum(x => x.ReceivedBytesPerSecond);
        public long SentBytesPerSecond => InterfaceMetrics.Sum(x => x.SentBytesPerSecond);

        public override string ToString()
            => string.Join(Environment.NewLine, InterfaceMetrics);
    }
}