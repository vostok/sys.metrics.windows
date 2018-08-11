using System;
// ReSharper disable InconsistentNaming

namespace Vostok.System.Metrics.Windows.Native.Flags
{
    [Flags]
    internal enum GetAdaptersAddressesFlags
    {
        GAA_FLAG_SKIP_UNICAST = 1,
        GAA_FLAG_SKIP_ANYCAST = 2,
        GAA_FLAG_SKIP_MULTICAST = 4,
        GAA_FLAG_SKIP_DNS_SERVER = 8,
        GAA_FLAG_INCLUDE_PREFIX = 16, // 0x00000010
        GAA_FLAG_SKIP_FRIENDLY_NAME = 32, // 0x00000020
        GAA_FLAG_INCLUDE_WINS_INFO = 64, // 0x00000040
        GAA_FLAG_INCLUDE_GATEWAYS = 128, // 0x00000080
        GAA_FLAG_INCLUDE_ALL_INTERFACES = 256, // 0x00000100
        GAA_FLAG_INCLUDE_ALL_COMPARTMENTS = 512, // 0x00000200
        GAA_FLAG_INCLUDE_TUNNEL_BINDINGORDER = 1024, // 0x00000400
    }
}