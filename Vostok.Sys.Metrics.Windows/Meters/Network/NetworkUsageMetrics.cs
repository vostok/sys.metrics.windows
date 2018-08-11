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

        public DataSize ReceivedPerSecond => DataSize.FromBytes(InterfaceMetrics.Sum(x => x.ReceivedPerSecond.Bytes));
        public DataSize SentPerSecond => DataSize.FromBytes(InterfaceMetrics.Sum(x => x.SentPerSecond.Bytes));

        public override string ToString()
            => string.Join(Environment.NewLine, InterfaceMetrics);
    }
}