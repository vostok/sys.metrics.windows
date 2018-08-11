using System;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters
{
    public static class InstanceNameProviders
    {
        public static readonly ICategoryInstanceNameProvider System = new CategoryInstanceNameProviders(false);
        public static readonly ICategoryInstanceNameProvider DotNet = new CategoryInstanceNameProviders(true);

        public static Func<string> Static(string instanceName)
            => () => instanceName;
    }
}