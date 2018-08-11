using BenchmarkDotNet.Running;

namespace Vostok.System.Metrics.Windows.Benchmark
{
    internal static class EntryPoint
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<InstanceNameUtilityBenchmark>();
        }
    }
}