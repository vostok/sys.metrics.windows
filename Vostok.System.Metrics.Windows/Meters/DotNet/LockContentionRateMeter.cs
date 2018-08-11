using System;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.PerformanceCounters;
using Vostok.System.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.System.Metrics.Windows.Meters.DotNet
{
    public class LockContentionRateMeter : IDisposable
    {
        public LockContentionRateMeter() : this(
            PerformanceCounterFactory.Default,
            InstanceNameProviders.DotNet.ForCurrentProcess())
        {}

        public LockContentionRateMeter(int pid) : this(
            PerformanceCounterFactory.Default,
            InstanceNameProviders.DotNet.ForPid(pid))
        {}

        private LockContentionRateMeter(IPerformanceCounterFactory counterFactory, Func<string> instanceNameProvider)
        {
            counter = counterFactory.CreateCounter(Category.ClrLocksAndThreads, "Contention Rate / Sec", instanceNameProvider);
        }

        public double GetContentionRate()
        {
            return counter.Observe();
        }

        public void Dispose()
        {
            counter.Dispose();
        }

        private readonly IPerformanceCounter<double> counter;
    }
}