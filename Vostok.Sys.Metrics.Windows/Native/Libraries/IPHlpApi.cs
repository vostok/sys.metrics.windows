using System;
using System.Runtime.InteropServices;
using Vostok.Sys.Metrics.Windows.Native.Flags;
using Vostok.Sys.Metrics.Windows.Native.Structures;

namespace Vostok.Sys.Metrics.Windows.Native.Libraries
{
    internal class IPHlpApi
    {
        private const string iphlpapi = "iphlpapi.dll";
        
        // IPHlpApi functions doesn't set error code
        
        [DllImport(iphlpapi)]
        public static extern unsafe int GetExtendedTcpTable(
            [Out] byte* pTcpTable,
            [In, Out] ref int pdwSize,
            [In] bool bOrder,
            [In] AddressFamily ulAf,
            [In] TCP_TABLE_CLASS TableClass,
            [In] uint Reserved
        );
        
        [DllImport(iphlpapi)]
        internal static extern unsafe int GetAdaptersAddresses(
            [In] AddressFamily family,
            [In] GetAdaptersAddressesFlags flags,
            [In] IntPtr pReserved,
            [In, Out] byte* adapterAddresses,
            [In, Out] ref int outBufLen);
    }
}