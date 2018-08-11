using System;
using System.Runtime.InteropServices;
using Vostok.System.Metrics.Windows.Native.Constants;
using Vostok.System.Metrics.Windows.Native.Flags;
using Vostok.System.Metrics.Windows.Native.Structures;
using FILETIME = Vostok.System.Metrics.Windows.Native.Structures.FILETIME;

namespace Vostok.System.Metrics.Windows.Native.Libraries
{
    internal static class Kernel32
    {
        private const string kernel32 = "kernel32.dll";

        [DllImport(kernel32, SetLastError = true)]
        public static extern bool IsWow64Process(
            [In] IntPtr handle,
            [Out] out bool wow64Process);

        [DllImport(kernel32, SetLastError = true)]
        public static extern IntPtr OpenProcess(
            [In] ProcessAccessRights dwDesiredAccess,
            [In] bool bInheritHandle,
            [In] int dwProcessId);

        [DllImport(kernel32, SetLastError = true)]
        public static extern bool GetExitCodeProcess(
            [In] IntPtr hProcess,
            [Out] out int lpExitCode
        );
        
        [DllImport(kernel32, SetLastError = true)]
        public static extern bool CloseHandle(
            [In] IntPtr handle
        );
        
        [DllImport(kernel32, SetLastError = true)]
        public static extern int GetProcessId(
            [In] IntPtr handle
        );
        
        [DllImport(kernel32)]
        public static extern IntPtr GetCurrentProcess();
        
        [DllImport(kernel32, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx(
            ref MEMORYSTATUSEX lpBuffer
        );
        
        [DllImport(kernel32, SetLastError = true)]
        public static extern bool GetProcessTimes(
            IntPtr hProcess,
            out FILETIME creationTime,
            out FILETIME exitTime,
            out FILETIME kernelTime,
            out FILETIME userTime);

        [DllImport(kernel32, SetLastError = true)]
        public static extern bool GetSystemTimes(
            out FILETIME idleTime,
            out FILETIME kernelTime,
            out FILETIME userTime);
        
        [DllImport(kernel32, SetLastError = true)]
        public static extern unsafe bool GetLogicalProcessorInformationEx(
            LOGICAL_PROCESSOR_RELATIONSHIP RelationshipType,
            SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX* Buffer,
            ref int                                   ReturnedLength
        );
    }
}