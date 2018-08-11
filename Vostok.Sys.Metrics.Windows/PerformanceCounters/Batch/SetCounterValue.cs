namespace Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch
{
    public delegate void SetCounterValue<T>(CounterContext<T> c, double value);
}