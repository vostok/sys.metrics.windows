using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Constants;
using Vostok.Sys.Metrics.Windows.Native.Flags;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;

namespace Vostok.Sys.Metrics.Windows.Native.Utilities
{
    internal class ActiveNetworkInterfaceCache
    {
        public static readonly ActiveNetworkInterfaceCache Instance = new ActiveNetworkInterfaceCache();

        private readonly TimeCache<HashSet<string>> cache;
        private readonly ResizeableBuffer resizeableBuffer = new ResizeableBuffer();
        private readonly object sync = new object();

        private ActiveNetworkInterfaceCache()
        {
            cache = new TimeCache<HashSet<string>>(() => BuildCache(), () => TimeSpan.FromSeconds(10));
        }

        public HashSet<string> GetUpInterfaces() => cache.GetValue();

        private unsafe HashSet<string> BuildCache()
        {
            var size = 1024;
            
            lock (sync)
            {
                while (true)
                {
                    var buffer = resizeableBuffer.Get(ref size, false);

                    fixed (byte* b = buffer)
                    {
                        var result = IPHlpApi.GetAdaptersAddresses(
                            AddressFamily.AF_UNSPEC,
                            GetAdaptersAddressesFlags.GAA_FLAG_INCLUDE_WINS_INFO |
                            GetAdaptersAddressesFlags.GAA_FLAG_INCLUDE_GATEWAYS |
                            GetAdaptersAddressesFlags.GAA_FLAG_INCLUDE_ALL_INTERFACES,
                            IntPtr.Zero,
                            b,
                            ref size);
                        if (result == (int) ErrorCodes.ERROR_BUFFER_OVERFLOW)
                            continue;
                        if (result == (int) ErrorCodes.ERROR_NO_DATA)
                            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        if (result != (int) ErrorCodes.ERROR_SUCCESS)
                            Win32ExceptionUtility.Throw(result);
                        
                        var upInterfaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        
                        for (var ptr = (IP_ADAPTER_ADDRESSES*) b; ptr != null; ptr = ptr->Next)
                        {
                            UnsafeHelper.CheckBounds(b, size, ptr, sizeof(IP_ADAPTER_ADDRESSES));
                            if (ptr->OperStatus == OperationalStatus.Up && ptr->IfType != NetworkInterfaceType.Loopback)
                                upInterfaces.Add(GetAdapterIdentity(new string(ptr->Description)));
                        }

                        return upInterfaces;
                    }
                }
            }            
        }

        public static string GetAdapterIdentity(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (var c in s)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(char.ToLowerInvariant(c));
            }

            return sb.ToString();
        }
    }
}