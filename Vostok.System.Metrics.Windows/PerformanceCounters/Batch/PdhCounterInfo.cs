using Vostok.System.Metrics.Windows.Native.Structures;

namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    internal struct PdhCounterInfo<T>
    {
        public CounterInfo<T> Info;
        public PdhCounter PdhCounter;
    }
}