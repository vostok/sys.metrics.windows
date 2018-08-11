using System.Net.NetworkInformation;
using BenchmarkDotNet.Attributes;
using Vostok.System.Metrics.Windows.Meters.Network;

namespace Vostok.System.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class TcpConnectionsMeterBechmark
    {
        private TcpConnectionsMeter meter = new TcpConnectionsMeter();
        private AllProcessesTcpConnectionsMeter allProcessesMeter = new AllProcessesTcpConnectionsMeter();

        [Benchmark]
        public void V4()
            => meter.GetStatisticsByTCPv4();
        
        [Benchmark]
        public void V6()
            => meter.GetStatisticsByTCPv6();
    
        [Benchmark]
        public void All()
            => meter.GetStatistics();
        
        [Benchmark]
        public void DotNet()
            => IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
        
        [Benchmark]
        public void AllProcesses()
            => allProcessesMeter.GetStatistics();
    }
}