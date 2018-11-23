using System;
using System.Runtime.InteropServices;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    /// <summary>
    /// <para>
    /// Provides information about memory utilization.
    /// </para>
    /// 
    /// <para>
    /// Internally uses WINAPI method GlobalMemoryStatusEx
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa366589(v=vs.85).aspx
    /// </para>
    /// </summary>
    public class HostMemoryMeter : IDisposable
    {
        public HostMemoryMeter()
        {}

        /// <summary>
        /// <para>
        /// Provides infromation about memory usage for a local machine
        /// </para>
        /// <para>This method is thread-safe</para>
        /// </summary>
        public HostMemoryInfo GetHostMemoryInfo()
        {
            var memoryStatusEx = GetMemoryStatusEx();
            return new HostMemoryInfo
            {
                TotalPhysicalMemoryBytes = (long) memoryStatusEx.ullTotalPhys,
                AvailablePhysicalMemoryBytes = (long) memoryStatusEx.ullAvailPhys,
            };
        }

        public void Dispose()
        {
        }

        #region WINAPI

        private MEMORYSTATUSEX GetMemoryStatusEx()
        {
            var memoryStatusExSize = checked((uint) Marshal.SizeOf(typeof(MEMORYSTATUSEX)));
            var result = new MEMORYSTATUSEX
            {
                dwLength = memoryStatusExSize
            };

            if (!Kernel32.GlobalMemoryStatusEx(ref result))
                Win32ExceptionUtility.Throw();
            
            return result;
        }

        private PERFORMANCE_INFORMATION GetPerformanceInformation()
        {
            if (!PsApi.GetPerformanceInfo(out var info))
                Win32ExceptionUtility.Throw();
            
            return info;
        }

        #endregion
    }
}

    