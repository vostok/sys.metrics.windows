using System;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.CPU
{
    /// <summary>
    /// <para>
    /// Measures CPU usage for a specific process.
    /// Works only for processes on a local machine.
    /// </para>
    /// 
    /// <para>Internally uses GetProcessTimes and GetSystemTimes from kernel32.dll.</para>
    /// 
    /// <para>
    /// CPU usage is measured as average from the last call to <see cref="GetCpuUsage"></see>
    /// Measuring CPU usage more frequently than once in 250 milliseconds causes incorrect results.
    /// That's why <see cref="GetCpuUsage"/> caches the value for next 250 ms.
    /// </para>
    /// </summary>
    public class ProcessCpuUsageMeter : IDisposable
    {
        /// <summary>
        /// Creates <see cref="ProcessCpuUsageMeter"/> for specified process.
        /// </summary>
        public ProcessCpuUsageMeter(int pid)
        {
            handle = ProcessUtility.OpenLimitedQueryInformationProcessHandle(pid);
            cpuUsageMeter = new CpuUsageMeter(() => CpuUsageNativeApi.GetProcessTime(handle));
        }

        /// <summary>
        /// Creates <see cref="ProcessCpuUsageMeter"/> for current process.
        /// </summary>
        public ProcessCpuUsageMeter()
            : this(ProcessUtility.CurrentProcessId)
        {
        }

        /// <summary>
        /// <para>
        /// Estimates current CPU usage for the process.
        /// CPU usage is averaged since last <see cref="GetCpuUsage"/> call.
        /// Measuring CPU usage more frequently than once in 250 milliseconds causes incorrect results.
        /// That's why <see cref="GetCpuUsage"/> caches the value for next 250 ms.
        /// </para>
        /// 
        /// <para>This method is thread-safe</para>
        /// </summary>
        /// <returns>Value from 0 to 1.</returns>
        public double GetCpuUsage()
        {
            return cpuUsageMeter.GetCpuUsage();
        }

        #region Dispose
        private void ReleaseUnmanagedResources()
        {
            Kernel32.CloseHandle(handle);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~ProcessCpuUsageMeter()
        {
            ReleaseUnmanagedResources();
        }
        #endregion

        private readonly IntPtr handle;
        private readonly CpuUsageMeter cpuUsageMeter;
    }
}