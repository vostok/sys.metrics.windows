namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    public interface IPerformanceCounterFactory
    {
        IPerformanceCounterBuilder<T> Create<T>() where T : new();
    }
}