using System;

namespace Vostok.System.Metrics.Windows.Meters.CPU
{
    /// <summary>
    /// <para>
    /// Measures CPU usage current host.
    /// Works only for processes on a local machine.
    /// </para>
    /// 
    /// <para>Internally uses GetSystemTimes from kernel32.dll.</para>
    /// 
    /// <para>
    /// CPU usage is measured as average from the last call to <see cref="GetHostCpuUsage"></see>
    /// Measuring CPU usage more frequently than once in 250 milliseconds causes incorrect results.
    /// That's why <see cref="GetHostCpuUsage"/> caches the value for next 250 ms.
    /// </para>
    /// </summary>
    public class HostCpuUsageMeterFast : IDisposable
    {
        public HostCpuUsageMeterFast()
        {
            cpuUsageMeter = new CpuUsageMeter(CpuUsageNativeApi.GetSystemUsedTime);
        }

        /// <summary>
        /// <para>
        /// Estimates current total CPU utilization.
        /// CPU usage is averaged since last <see cref="GetHostCpuUsage"/> call.
        /// Measuring CPU usage more frequently than once in 250 milliseconds causes incorrect results.
        /// That's why <see cref="GetHostCpuUsage"/> caches the value for next 250 ms.
        /// </para>
        /// 
        /// <para>This method is thread-safe</para>
        /// </summary>
        /// <returns>Value from 0 to 1.</returns>
        public double GetHostCpuUsage()
        {
            return cpuUsageMeter.GetCpuUsage();
        }

        public void Dispose()
        {
        }

        private readonly CpuUsageMeter cpuUsageMeter;
    }
}