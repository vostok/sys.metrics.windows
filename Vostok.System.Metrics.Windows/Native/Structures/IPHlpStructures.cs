using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Vostok.System.Metrics.Windows.Native.Constants;

// ReSharper disable InconsistentNaming

namespace Vostok.System.Metrics.Windows.Native.Structures
{
    internal enum TCP_TABLE_CLASS
    {
        TCP_TABLE_BASIC_LISTENER,
        TCP_TABLE_BASIC_CONNECTIONS,
        TCP_TABLE_BASIC_ALL,
        TCP_TABLE_OWNER_PID_LISTENER,
        TCP_TABLE_OWNER_PID_CONNECTIONS,
        TCP_TABLE_OWNER_PID_ALL,
        TCP_TABLE_OWNER_MODULE_LISTENER,
        TCP_TABLE_OWNER_MODULE_CONNECTIONS,
        TCP_TABLE_OWNER_MODULE_ALL
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_TCPROW_OWNER_PID
    {
        public MIB_TCP_STATE dwState;
        public int dwLocalAddr;
        public int dwLocalPort;
        public int dwRemoteAddr;
        public int dwRemotePort;
        public int dwOwningPid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_TCPROW
    {
        public MIB_TCP_STATE dwState;
        public int dwLocalAddr;
        public int dwLocalPort;
        public int dwRemoteAddr;
        public int dwRemotePort;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct MIB_TCP6ROW_OWNER_PID
    {
        public fixed byte ucLocalAddr[16];
        public int dwLocalScopeId;
        public int dwLocalPort;
        public fixed byte ucRemoteAddr[16];
        public int dwRemoteScopeId;
        public int dwRemotePort;
        public MIB_TCP_STATE dwState;
        public int dwOwningPid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct IP_ADAPTER_ADDRESSES
    {
        public uint Length;
        public uint IfIndex;
        public IP_ADAPTER_ADDRESSES* Next;
        public byte* AdapterName;
        public IntPtr FirstUnicastAddress; // PIP_ADAPTER_UNICAST_ADDRESS
        public IntPtr FirstAnycastAddress; // PIP_ADAPTER_ANYCAST_ADDRESS
        public IntPtr FirstMulticastAddress; // PIP_ADAPTER_MULTICAST_ADDRESS
        public IntPtr FirstDnsServerAddress; // PIP_ADAPTER_DNS_SERVER_ADDRESS
        public char* DnsSuffix;
        public char* Description;
        public char* FriendlyName;
        public fixed byte PhysicalAddress[8];
        public uint PhysicalAddressLength;
        public uint Flags;
        public uint Mtu;
        public NetworkInterfaceType IfType;
        public OperationalStatus OperStatus;
        public uint Ipv6IfIndex;
        public fixed uint ZoneIndices[16];
        public IntPtr FirstPrefix; // PIP_ADAPTER_PREFIX
        public ulong TransmitLinkSpeed;
        public ulong ReceiveLinkSpeed;
        public IntPtr FirstWinsServerAddress; // PIP_ADAPTER_WINS_SERVER_ADDRESS_LH
        public IntPtr FirstGatewayAddress; // PIP_ADAPTER_GATEWAY_ADDRESS_LH
        public uint Ipv4Metric;
        public uint Ipv6Metric;
        public ulong Luid; // IF_LUID
        public SOCKET_ADDRESS Dhcpv4Server;
        public uint CompartmentId; // NET_IF_COMPARTMENT_ID
        public fixed byte NetworkGuid[16]; // NET_IF_NETWORK_GUID
        public int ConnectionType; // NET_IF_CONNECTION_TYPE
        public int TunnelType; // TUNNEL_TYPE
        public SOCKET_ADDRESS Dhcpv6Server;
        public fixed byte Dhcpv6ClientDuid[130];
        public uint Dhcpv6ClientDuidLength;
        public uint Dhcpv6Iaid;
        public IntPtr FirstDnsSuffix; // PIP_ADAPTER_DNS_SUFFIX
    }

    internal struct SOCKET_ADDRESS
    {
        private IntPtr lpSockaddr; // LPSOCKADDR
        private int iSockaddrLength;
    }
}