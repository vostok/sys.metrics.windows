using System;
using System.Collections.Generic;

namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    internal class PerformanceCounterBuilder<T> : IPerformanceCounterBuilder<T>  where T : new()
    {
        private readonly PerformanceCounterBuilder<T> previous;
        private readonly CounterInfo<T> counter;
        private readonly IPerformanceCounterFactoryInternal performanceCounterFactory;

        private PerformanceCounterBuilder(PerformanceCounterBuilder<T> previous,
            IPerformanceCounterFactoryInternal performanceCounterFactory,
            CounterInfo<T> counter)
        {
            this.performanceCounterFactory = performanceCounterFactory;
            this.previous = previous;
            this.counter = counter;
        }

        public PerformanceCounterBuilder(IPerformanceCounterFactoryInternal performanceCounterFactory)
            => this.performanceCounterFactory = performanceCounterFactory;

        public IPerformanceCounterBuilder<T> WithCounter(string categoryName, string counterName, SetCounterValue<T> setValue)
        {
            return new PerformanceCounterBuilder<T>(this, performanceCounterFactory, new CounterInfo<T>
            {
                Name = new FullCounterName(categoryName, counterName),
                SetValue = setValue
            });
        }

        public IPerformanceCounter<T> Build()
            => new BatchPerformanceCounter<T>(GetCounters());

        public IPerformanceCounter<T> Build(Func<string> instanceNameProvider)
            => performanceCounterFactory.Create(instanceNameProvider, GetCounters());

        public IPerformanceCounter<T> Build(string instanceName)
            => performanceCounterFactory.Create(instanceName, GetCounters());

        public IPerformanceCounter<T[]> BuildWildcard(string instanceNameWildcard, Action<CounterContext<T>, string> setInstanceName)
            => performanceCounterFactory.CreateWildcard(instanceNameWildcard, GetCounters(), setInstanceName);

        private bool HasCounter => counter.Name.CounterName != null;
        
        private CounterInfo<T>[] GetCounters()
        {
            var counters = new List<CounterInfo<T>>();
            var node = this;
            while (node.HasCounter)
            {
                counters.Add(node.counter);
                node = node.previous;
            }

            return counters.ToArray();
        }
    }
}