using System;

namespace Vostok.System.Metrics.Windows.PerformanceCounters
{
    public interface IPerformanceCounter<T> : IDisposable
    {
        T Observe();
    }
}