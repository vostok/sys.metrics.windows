using System;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters
{
    public class PerformanceCounterFactory_Old : IPerformanceCounterFactory_Old
    {
        private readonly bool usePdh;
        public static readonly IPerformanceCounterFactory_Old Default = new PerformanceCounterFactory_Old();

        public PerformanceCounterFactory_Old()
            : this(true) { }

        internal PerformanceCounterFactory_Old(bool usePdh)
        {
            this.usePdh = usePdh;
        }

        public IPerformanceCounter<double> Create(string categoryName, string counterName, Func<string> instanceNameProvider)
        {
            return new DynamicPerformanceCounter<double>(instanceName => Create(categoryName, counterName, instanceName), instanceNameProvider);
        }

        public IPerformanceCounter<double> Create(string categoryName, string counterName, string instanceName)
        {
            if (usePdh)
                return new PdhPerformanceCounter_Old(categoryName, counterName, instanceName);
            
            return new StaticPerformanceCounter_Old(categoryName, counterName, instanceName);
        }

        public IPerformanceCounter<double> Create(string categoryName, string counterName)
        {
            if (usePdh)
                return new PdhPerformanceCounter_Old(categoryName, counterName);
            return new StaticPerformanceCounter_Old(categoryName, counterName);
        }
    }
}