using System.IO;
using BenchmarkDotNet.Attributes;
using Vostok.Sys.Metrics.Windows.Benchmark.Implementations;
using Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters;
using Vostok.Sys.Metrics.Windows.Meters.Disk;

namespace Vostok.Sys.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class LogicalDiskMeterBechmark
    {
        private LogicalDiskMeter_Old dotNet = new LogicalDiskMeter_Old(null, new PerformanceCounterFactory_Old(false));
        private LogicalDiskMeter_Old pdh = new LogicalDiskMeter_Old(null, new PerformanceCounterFactory_Old(true));
        private LogicalDiskMeter pdhBatch = new LogicalDiskMeter('C');
        private AllLogicalDisksMeter pdhArray = new AllLogicalDisksMeter();

        [Benchmark]
        public void DotNet()
            => dotNet.GetDiskMetrics();
        
        [Benchmark]
        public void Pdh()
            => pdh.GetDiskMetrics();
        
        [Benchmark]
        public void PdhBatch()
            => pdhBatch.GetDiskMetrics();
        
        [Benchmark]
        public void PdhArray()
            => pdhArray.GetDiskMetrics();

        [Benchmark]
        public void Drive()
        {
            DriveInfo
                .GetDrives();
        }
    }
}