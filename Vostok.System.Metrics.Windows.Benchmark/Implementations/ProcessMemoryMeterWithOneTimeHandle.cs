using System;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.Meters.Memory;
using Vostok.System.Metrics.Windows.Native.Constants;
using Vostok.System.Metrics.Windows.Native.Libraries;
using Vostok.System.Metrics.Windows.Native.Utilities;

namespace Vostok.System.Metrics.Windows.Benchmark.Implementations
{
    /// <summary>
    /// <para>
    /// Estimates memory consumption for a given process.
    /// Reported values do not include child process consumption.
    /// </para>
    /// 
    /// <para>
    /// Internally uses GetProcessMemoryInfo WinApi function
    /// </para>
    /// </summary>
    public class ProcessMemoryMeterWithOneTimeHandle : IDisposable
    {
        /// <summary>
        /// Estimates memory consumption
        /// </summary>
        /// <exception cref="InvalidOperationException">Process has exited</exception>
        /// <returns></returns>
        public ProcessMemoryInfo GetMemoryInfo(int pid)
        {
            var handle = IntPtr.Zero;
            try
            {
                handle = ProcessUtility.OpenLimitedQueryInformationProcessHandle(pid);
                EnsureProcessIsRunning(handle, pid);

                if (!PsApi.GetProcessMemoryInfo(handle, out var counters))
                    Win32ExceptionUtility.Throw();

                return new ProcessMemoryInfo
                {
                    // use PrivateUsage instead of PagefileUsage because PagefileUsage is always zero on Windows 7, Windows Server 2008 R2 and earlier systems
                    Private = DataSize.FromBytes((long) counters.PrivateUsage),
                    WorkingSet = DataSize.FromBytes((long) counters.WorkingSetSize)
                };
            }
            finally
            {
                Kernel32.CloseHandle(handle);
            }
        }

        private static void EnsureProcessIsRunning(IntPtr handle, int pid)
        {
            if (!Kernel32.GetExitCodeProcess(handle, out var exitCode))
                Win32ExceptionUtility.Throw();

            if (exitCode != ProcessExitCodes.STILL_ALIVE)
                ThrowOnProcessExit(pid);
        }
        
        private static void ThrowOnProcessExit(int pid)
            => throw new InvalidOperationException(
                $"Process with pid {pid} has exited, so the memory metrics is not available.");

        public void Dispose()
        {
        }
    }
}