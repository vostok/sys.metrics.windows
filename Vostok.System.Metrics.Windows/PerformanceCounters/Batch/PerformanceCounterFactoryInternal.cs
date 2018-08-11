using System;

namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    internal class PerformanceCounterFactoryInternal : IPerformanceCounterFactoryInternal
    {
        public IPerformanceCounterBuilder<T> Create<T>() where T : new()
            => new PerformanceCounterBuilder<T>(this);

        public IPerformanceCounter<T> Create<T>(Func<string> instanceNameProvider, CounterInfo<T>[] counters) where T : new()
            => new DynamicPerformanceCounter<T>(name => Create(name, counters), instanceNameProvider);

        public IPerformanceCounter<T> Create<T>(string instanceName, CounterInfo<T>[] counters) where T : new()
            => new BatchPerformanceCounter<T>(counters, instanceName);

        public IPerformanceCounter<T> Create<T>(CounterInfo<T>[] counters) where T : new()
            => new BatchPerformanceCounter<T>(counters);

        public IPerformanceCounter<T[]> CreateWildcard<T>(string instanceNameWildcard, CounterInfo<T>[] counters, Action<CounterContext<T>, string> setInstanceName) where T : new()
            => new ArrayPerformanceCounter<T>(counters, instanceNameWildcard, setInstanceName);
    }
}