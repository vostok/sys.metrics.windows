using System;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.CPU
{
    internal static class CpuUsageNativeApi
    {
        public static ulong GetSystemTime()
        {
            if (Kernel32.GetSystemTimes(out var _, out var kernelTime, out var userTime))
                return kernelTime.ToULong() + userTime.ToULong();

            Win32ExceptionUtility.Throw();
            // unreachable
            return 0;
        }

        public static ulong GetSystemUsedTime()
        {
            if (Kernel32.GetSystemTimes(out var idleTime, out var kernelTime, out var userTime))
                return kernelTime.ToULong() + userTime.ToULong() - idleTime.ToULong();

            Win32ExceptionUtility.Throw();
            // unreachable
            return 0;
        }

        public static ulong GetProcessTime(IntPtr hProcess)
        {
            if (Kernel32.GetProcessTimes(hProcess, out var _, out var _, out var kernelTime, out var userTime))
                return kernelTime.ToULong() + userTime.ToULong();

            Win32ExceptionUtility.Throw();
            // unreachable
            return 0;
        }
    }
}