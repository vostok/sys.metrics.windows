namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    internal struct CounterInfo<T>
    {
        public FullCounterName Name;
        public SetCounterValue<T> SetValue;
    }
}