using System;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch
{
    internal interface IPerformanceCounterFactoryInternal : IPerformanceCounterFactory
    {
        IPerformanceCounter<T> Create<T>(Func<string> instanceNameProvider, CounterInfo<T>[] counters) where T : new();
        IPerformanceCounter<T> Create<T>(string instanceName, CounterInfo<T>[] counters) where T : new();
        IPerformanceCounter<T> Create<T>(CounterInfo<T>[] counters) where T : new();
        IPerformanceCounter<T[]> CreateWildcard<T>(string instanceNameWildcard, CounterInfo<T>[] counters,
            Action<CounterContext<T>, string, int> setInstanceName) where T : new();
    }
}