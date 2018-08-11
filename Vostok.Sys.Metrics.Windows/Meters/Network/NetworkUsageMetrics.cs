using System;
using System.Linq;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    public struct NetworkUsageMetrics
    {
        public NetworkUsageMetrics(InterfaceUsageMetrics[] interfaceMetrics)
            => InterfaceMetrics = interfaceMetrics;

        public InterfaceUsageMetrics[] InterfaceMetrics { get; }

        public long ReceivedBytesPerSecond => InterfaceMetrics.Sum(x => x.ReceivedPerSecondBytes);
        public long SentBytesPerSecond => InterfaceMetrics.Sum(x => x.SentPerSecondBytes);

        public override string ToString()
            => string.Join(Environment.NewLine, InterfaceMetrics);
    }
}