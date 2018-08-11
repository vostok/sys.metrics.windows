using System;

namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    public interface IPerformanceCounterBuilder<T>  where T : new()
    {
        IPerformanceCounterBuilder<T> WithCounter(string categoryName, string counterName, SetCounterValue<T> setValue);

        IPerformanceCounter<T> Build();
        IPerformanceCounter<T> Build(string instanceName);
        IPerformanceCounter<T> Build(Func<string> instanceNameProvider);
        IPerformanceCounter<T[]> BuildWildcard(string instanceNameWildcard, Action<CounterContext<T>, string> setInstanceName);
    }
}