using System;
using Vostok.Sys.Metrics.PerfCounters;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters
{
    public interface IPerformanceCounterFactory_Old
    {
        IPerformanceCounter<double> Create(string categoryName, string counterName, Func<string> instanceNameProvider);
        IPerformanceCounter<double> Create(string categoryName, string counterName, string instanceName);
        IPerformanceCounter<double> Create(string categoryName, string counterName);
    }
}