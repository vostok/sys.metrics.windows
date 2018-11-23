namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters
{
    internal static class CounterPathFactory
    {
        public static string Create(string categoryName, string counterName, string instanceName)
            => $@"\{categoryName}({instanceName})\{counterName}";

        public static string Create(string categoryName, string counterName)
            => $@"\{categoryName}\{counterName}";

        public static string CreateForAllInstances(string categoryName, string counterName)
            => $@"\{categoryName}(*)\{counterName}";
    }
}