using System;
using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.CPU
{
    internal class CpuUsageMeter
    {
        private readonly Func<ulong> getCurrentUsedTime;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getCurrentUsedTime">Provides used time in a 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601 (UTC).</param>
        public CpuUsageMeter(Func<ulong> getCurrentUsedTime)
        {
            this.getCurrentUsedTime = getCurrentUsedTime;
            this.cache = new TimeCache(Measure, () => TimeSpan.FromMilliseconds(250));
        }

        public double GetCpuUsage()
        {
            return cache.GetValue();
        }

        private double Measure()
        {
            var systemTime = CpuUsageNativeApi.GetSystemTime();
            var usedTime = getCurrentUsedTime();

            if (isFirstMeasure)
            {
                sysTimePrev = systemTime;
                usedTimePrev = usedTime;
                isFirstMeasure = false;
                return 0;
            }

            var sysDelta = systemTime - sysTimePrev;
            var usedDelta = usedTime - usedTimePrev;

            sysTimePrev = systemTime;
            usedTimePrev = usedTime;

            var usage = sysDelta <= 0
                ? 0
                : Math.Max(0, Math.Min(1, usedDelta / (double) sysDelta));

            return usage;
        }

        private ulong usedTimePrev;
        private ulong sysTimePrev;

        private bool isFirstMeasure = true;
        private readonly TimeCache cache;
    }
}