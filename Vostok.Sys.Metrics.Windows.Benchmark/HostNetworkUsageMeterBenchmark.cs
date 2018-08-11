using BenchmarkDotNet.Attributes;
using Vostok.Sys.Metrics.Windows.Benchmark.Implementations;
using Vostok.Sys.Metrics.Windows.Meters.Network;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.Sys.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class HostNetworkUsageMeterBenchmark
    {
        private HostNetworkUsageMeter_BatchCounter batchCounter = new HostNetworkUsageMeter_BatchCounter();
        private HostNetworkUsageMeter arrayCounter = new HostNetworkUsageMeter();

        [Benchmark]
        public void BatchCounter() => batchCounter.GetNetworkMetrics();
        [Benchmark]
        public void ArrayCounter() => arrayCounter.GetNetworkMetrics();
    }
}