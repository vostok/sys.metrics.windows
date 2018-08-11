namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    public delegate void SetCounterValue<T>(CounterContext<T> c, double value);
}