using BenchmarkDotNet.Attributes;
using Vostok.System.Metrics.Windows.Meters;

namespace Vostok.System.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class GetAllProcessesBenchmark
    {
        private ProcessHelper helper;

        [GlobalSetup]
        public void Setup()
        {
            helper = new ProcessHelper();
        }

        [Benchmark]
        public void NtQuery()
        {
            var result = helper.GetAllProcesses();
        }

        [Benchmark]
        public void Process()
        {
            var result = global::System.Diagnostics.Process.GetProcesses();
        }
    }
}