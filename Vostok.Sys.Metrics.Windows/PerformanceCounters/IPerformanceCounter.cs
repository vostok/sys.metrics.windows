using System;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters
{
    public interface IPerformanceCounter<T> : IDisposable
    {
        T Observe();
    }
}