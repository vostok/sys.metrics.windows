using Vostok.Sys.Metrics.PerfCounters;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters
{
    internal class StaticPerformanceCounter_Old : IPerformanceCounter<double>
    {
        private readonly System.Diagnostics.PerformanceCounter counter;
        private readonly object syncObject;

        public StaticPerformanceCounter_Old(string categoryName, string counterName, string instanceName=null)
        {
            counter = instanceName == null
                ? new System.Diagnostics.PerformanceCounter(categoryName, counterName, true)
                : new System.Diagnostics.PerformanceCounter(categoryName, counterName, instanceName, true);
            syncObject = new object();
        }
        
        public StaticPerformanceCounter_Old(string categoryName, string counterName)
        {
            counter = new System.Diagnostics.PerformanceCounter(categoryName, counterName, true);
            syncObject = new object();
        }

        public double Observe()
        {
            lock (syncObject)
            {
                // NextValue is not thread-safe
                return counter.NextValue();
            }
        }

        public void Dispose()
        {
            counter.Dispose();
        }
    }
}