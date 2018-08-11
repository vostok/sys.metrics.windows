using System;
using Vostok.System.Metrics.Windows.Native.Utilities;

namespace Vostok.System.Metrics.Windows.PerformanceCounters
{
    public class CategoryInstanceNameProviders : ICategoryInstanceNameProvider
    {
        private readonly bool forDotNetCounters;

        internal CategoryInstanceNameProviders(bool forDotNetCounters)
        {
            this.forDotNetCounters = forDotNetCounters;
            currentProcessInstanceNameProvider =
                ProcessInstanceNamesCache.Instance.ForPid(ProcessUtility.CurrentProcessId, forDotNetCounters);
        }

        public Func<string> ForPid(int pid)
            => pid == ProcessUtility.CurrentProcessId ? ForCurrentProcess() : ProcessInstanceNamesCache.Instance.ForPid(pid, forDotNetCounters);

        public Func<string> ForCurrentProcess()
            => currentProcessInstanceNameProvider;
        
        private readonly Func<string> currentProcessInstanceNameProvider;
    }
}