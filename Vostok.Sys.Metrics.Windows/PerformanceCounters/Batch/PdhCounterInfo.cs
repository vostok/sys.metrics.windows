using Vostok.Sys.Metrics.Windows.Native.Structures;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch
{
    internal struct PdhCounterInfo<T>
    {
        public CounterInfo<T> Info;
        public PdhCounter PdhCounter;
    }
}