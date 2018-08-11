using System.Runtime.InteropServices;

namespace Vostok.System.Metrics.Windows.Native.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct FILETIME
    {
        /// <summary>Specifies the low 32 bits of the <see langword="FILETIME" />.</summary>
        public uint dwLowDateTime;
        /// <summary>Specifies the high 32 bits of the <see langword="FILETIME" />.</summary>
        public uint dwHighDateTime;
    }

    internal static class FILETIME_Extensions
    {
        public static ulong ToULong(this FILETIME filetime)
        {
            var high = (ulong)filetime.dwHighDateTime;
            var low = (ulong)filetime.dwLowDateTime;
            return (high << 32) | low;
        }
    }
}