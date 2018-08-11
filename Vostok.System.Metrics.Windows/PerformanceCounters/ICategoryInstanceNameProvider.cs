using System;

namespace Vostok.System.Metrics.Windows.PerformanceCounters
{
    public interface ICategoryInstanceNameProvider
    {
        Func<string> ForPid(int pid);
        Func<string> ForCurrentProcess();
    }
}