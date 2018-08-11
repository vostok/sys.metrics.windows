using System;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Utilities;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.Sys.Metrics.Windows.Meters.DotNet
{
    public class DotNetThreadsMeter : IDisposable
    {
        private readonly IPerformanceCounter<double> physicalThreadsCounter;

        public DotNetThreadsMeter()
            : this(ProcessUtility.CurrentProcessId)
        { 
        }

        public DotNetThreadsMeter(int pid)
            : this(
                PerformanceCounterFactory.Default,
                InstanceNameProviders.DotNet.ForPid(pid))
        {
        }

        private DotNetThreadsMeter(IPerformanceCounterFactory factory, Func<string> instanceNameProvider)
        {
            physicalThreadsCounter = factory.CreateCounter(
                Category.ClrLocksAndThreads, "# of current physical Threads", instanceNameProvider);
        }

        public int GetDotNetPhysicalThreadsCount()
        {
            return (int) physicalThreadsCounter.Observe();
        }

        public void Dispose()
        {
            physicalThreadsCounter.Dispose();
        }
    }
}
