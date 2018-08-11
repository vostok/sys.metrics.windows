using System;

namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    public static class PerformanceCounterFactoryExtensions
    {
        public static IPerformanceCounter<double> CreateCounter(
            this IPerformanceCounterFactory factory,
            string categoryName, string counterName, Func<string> instanceNameProvider)
        {
            return factory
                .Create<double>()
                .WithCounter(categoryName, counterName, (c, r) => c.Result = r)
                .Build(instanceNameProvider);
        }

        public static IPerformanceCounter<double> CreateCounter(
            this IPerformanceCounterFactory factory,
            string categoryName, string counterName, string instanceName)
        {
            return factory
                .Create<double>()
                .WithCounter(categoryName, counterName, (c, r) => c.Result = r)
                .Build(instanceName);
        }

        public static IPerformanceCounter<double> CreateCounter(
            this IPerformanceCounterFactory factory,
            string categoryName, string counterName)
        {
            return factory
                .Create<double>()
                .WithCounter(categoryName, counterName, (c, r) => c.Result = r)
                .Build();
        }
    }
}