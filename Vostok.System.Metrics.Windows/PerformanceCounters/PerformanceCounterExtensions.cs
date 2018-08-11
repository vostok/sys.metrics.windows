namespace Vostok.System.Metrics.Windows.PerformanceCounters
{
    public static class PerformanceCounterExtensions
    {
        public static bool TryObserve<T>(this IPerformanceCounter<T> counter, out T value)
        {
            try
            {
                value = counter.Observe();
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }
    }
}