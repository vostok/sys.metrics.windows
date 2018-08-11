using System;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
{
    public struct DisksMetrics
    {
        public DisksMetrics(DiskMetrics[] metrics)
        {
            Metrics = metrics;
        }

        public DiskMetrics[] Metrics { get; }

        public override string ToString()
            => string.Join(Environment.NewLine, (object[]) Metrics);
    }
}