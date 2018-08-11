using System;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
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
    public class ProcessMemoryMeter : IDisposable
    {
        private readonly IntPtr handle;
        private readonly int pid;

        /// <summary>
        /// Creates ProcessMemoryMeter
        /// </summary>
        /// <param name="processId"></param>
        public ProcessMemoryMeter(int processId)
        {
            pid = processId;
            handle = ProcessUtility.OpenLimitedQueryInformationProcessHandle(processId);
        }

        /// <summary>
        /// Creates ProcessMemoryMeter for current process
        /// </summary>
        public ProcessMemoryMeter()
            : this(ProcessUtility.CurrentProcessId)
        {
        }

        /// <summary>
        /// Estimates memory consumption
        /// </summary>
        /// <exception cref="InvalidOperationException">Process has exited</exception>
        /// <returns></returns>
        public ProcessMemoryInfo GetMemoryInfo()
        {
            ProcessUtility.EnsureProcessIsRunning(handle, pid);
            
            if (!PsApi.GetProcessMemoryInfo(handle, out var counters))
                Win32ExceptionUtility.Throw();
            
            return new ProcessMemoryInfo
            {
                // use PrivateUsage instead of PagefileUsage because PagefileUsage is always zero on Windows 7, Windows Server 2008 R2 and earlier systems
                PrivateBytes = (long) counters.PrivateUsage,
                WorkingSetBytes = (long) counters.WorkingSetSize,
            };
        }

        #region Dispose
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
        
        private void ReleaseUnmanagedResources()
        {
            Kernel32.CloseHandle(handle);
        }

        ~ProcessMemoryMeter()
        {
            ReleaseUnmanagedResources();
        }
        #endregion
    }
}