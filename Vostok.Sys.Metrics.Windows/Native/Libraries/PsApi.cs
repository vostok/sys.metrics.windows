using System;
using System.Runtime.InteropServices;
using Vostok.Sys.Metrics.Windows.Native.Structures;

namespace Vostok.Sys.Metrics.Windows.Native.Libraries
{
    internal static class PsApi
    {
        private const string psapi = "psapi.dll";

        public static unsafe bool GetProcessMemoryInfo(IntPtr process, out PROCESS_MEMORY_COUNTERS_EX memoryCountersEx)
            => GetProcessMemoryInfo(process, out memoryCountersEx, sizeof(PROCESS_MEMORY_COUNTERS_EX));

        [Obsolete("Slow: ~5 milliseconds per call")]
        public static unsafe bool GetPerformanceInfo(out PERFORMANCE_INFORMATION performanceInfo)
            => GetPerformanceInfo(out performanceInfo, sizeof(PERFORMANCE_INFORMATION));
        
        [DllImport(psapi, SetLastError = true)]
        private static extern bool GetProcessMemoryInfo(
            [In] IntPtr process,
            [Out] out PROCESS_MEMORY_COUNTERS_EX ppsmemCounters,
            [In] int cb);
        
        [DllImport(psapi, SetLastError = true)]
        private static extern bool GetPerformanceInfo(
            out PERFORMANCE_INFORMATION pPerformanceInformation,
            [In] int cb
        );
    }
}