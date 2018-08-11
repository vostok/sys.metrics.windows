using System;
using System.Runtime.InteropServices;
using Vostok.System.Metrics.Windows.Native.Constants;

namespace Vostok.System.Metrics.Windows.Native.Libraries
{
    internal static class NtDll
    {
        private const string ntdll = "ntdll.dll";
        
        [DllImport(ntdll)]
        public static extern NtStatus NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS query, IntPtr dataPtr, int size, out int returnedSize);
    }
}