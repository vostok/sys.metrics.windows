namespace Vostok.System.Metrics.Windows.PerformanceCounters.Batch
{
    public static class PerformanceCounterFactory
    {
        public static readonly IPerformanceCounterFactory Default = new PerformanceCounterFactoryInternal();
    }
}