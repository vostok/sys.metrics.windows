using BenchmarkDotNet.Attributes;
using Vostok.System.Metrics.Windows.Benchmark.Implementations;
using Vostok.System.Metrics.Windows.Meters.Network;
using Vostok.System.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.System.Metrics.Windows.Benchmark
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