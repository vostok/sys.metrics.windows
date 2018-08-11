using System;
using System.Collections.Generic;
using System.Net;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.Helpers.Pool;
using Vostok.System.Metrics.Windows.Meters.Network;
using Vostok.System.Metrics.Windows.Native.Constants;
using Vostok.System.Metrics.Windows.Native.Libraries;
using Vostok.System.Metrics.Windows.Native.Structures;

namespace Vostok.System.Metrics.Windows.Native.Utilities
{
    internal class NetworkUtility
    {
        private static readonly Pool<ResizeableBuffer> ArrayBufferPoolV4 = new Pool<ResizeableBuffer>(() => new ResizeableBuffer());
        private static readonly Pool<ResizeableBuffer> ArrayBufferPoolV6 = new Pool<ResizeableBuffer>(() => new ResizeableBuffer());
        
        public static void GetTcpStats(AddressFamily addressFamily, TcpStateStatistics stats, int? pidFilter=null, int? portFilter=null)
        {
            ThrowOnUnknownAddressFamily(addressFamily);

            var pool = addressFamily == AddressFamily.AF_INET ? ArrayBufferPoolV4 : ArrayBufferPoolV6;

            using (var arrayBuffer = pool.AcquireHandle())
            {
                GetTcpTableWithPid(addressFamily, arrayBuffer);
                GetStatistics(arrayBuffer.Resource.Buffer, addressFamily, stats, pidFilter, portFilter);
            }
        }

        public static void GetAllTcpStats(AddressFamily addressFamily, Dictionary<int, TcpStateStatistics> stats)
        {
            ThrowOnUnknownAddressFamily(addressFamily);

            var pool = addressFamily == AddressFamily.AF_INET ? ArrayBufferPoolV4 : ArrayBufferPoolV6;

            using (var arrayBuffer = pool.AcquireHandle())
            {
                GetTcpTableWithPid(addressFamily, arrayBuffer);
                GetStatisticsForAllProcesses(arrayBuffer.Resource.Buffer, addressFamily, stats);
            }
        }

        private static void ThrowOnUnknownAddressFamily(AddressFamily addressFamily)
        {
            if (addressFamily != AddressFamily.AF_INET && addressFamily != AddressFamily.AF_INET6)
                throw new ArgumentException(
                    $"TcpStats available only for AddressFamily.AF_INET (2) and AddressFamily.AF_INET6 (23), but found: {addressFamily}",
                    nameof(addressFamily));
        }

        private static unsafe void GetTcpTableWithPid(AddressFamily addressFamily, ResizeableBuffer resizeableBuffer)
        {
            var size = EstimateBufferSize(addressFamily, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);

            var buffer = resizeableBuffer.Get(ref size);

            while (true)
            {
                fixed (byte* b = buffer)
                {
                    var result = IPHlpApi.GetExtendedTcpTable(b, ref size, false, addressFamily,
                        TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
                    if (result == (int) ErrorCodes.ERROR_INSUFFICIENT_BUFFER)
                    {
                        buffer = resizeableBuffer.Get(ref size);
                        continue;
                    }

                    if (result != 0)
                        Win32ExceptionUtility.Throw(result);

                    var itemSize = addressFamily == AddressFamily.AF_INET
                        ? sizeof(MIB_TCPROW_OWNER_PID)
                        : sizeof(MIB_TCP6ROW_OWNER_PID);
                    
                    var itemCount = *(int*) b;
                    if (itemCount * itemSize + 4 > buffer.Length)
                        throw new InvalidOperationException($"Buffer overflow check failed. ItemCount: {itemCount}, ItemSize: {itemSize}, BufferSize: {buffer.Length}");
                    
                    return;
                }
            }
        }

        private static unsafe TcpStateStatistics GetStatistics(byte[] data, AddressFamily addressFamily,
            TcpStateStatistics stats, int? pidFilter, int? portFilter)
        {
            fixed (byte* b = data)
            {
                var count = *(int*) b;
                if (addressFamily == AddressFamily.AF_INET)
                {
                    var ptr = (MIB_TCPROW_OWNER_PID*) (b + 4);

                    for (var i = 0; i < count; i++)
                        AddToStats(ref stats, ptr[i].dwState, ptr[i].dwOwningPid, ptr[i].dwLocalPort, pidFilter, portFilter);

                    return stats;
                }

                if (addressFamily == AddressFamily.AF_INET6)
                {
                    var ptr = (MIB_TCP6ROW_OWNER_PID*) (b + 4);

                    for (var i = 0; i < count; i++)
                        AddToStats(ref stats, ptr[i].dwState, ptr[i].dwOwningPid, ptr[i].dwLocalPort, pidFilter, portFilter);

                    
                }
                return stats;
            }
        }

        private static unsafe void GetStatisticsForAllProcesses(byte[] data, AddressFamily addressFamily, Dictionary<int,TcpStateStatistics> stats)
        {            
            fixed (byte* b = data)
            {
                var count = *(int*) b;
                if (addressFamily == AddressFamily.AF_INET)
                {
                    var ptr = (MIB_TCPROW_OWNER_PID*) (b + 4);

                    for (var i = 0; i < count; i++)
                    {
                        var pid = ptr[i].dwOwningPid;
                        if (!stats.TryGetValue(pid, out var stat))
                            stats[pid] = stat = new TcpStateStatistics();
                        Add(stat, ptr[i].dwState);
                    }
                    
                }

                if (addressFamily == AddressFamily.AF_INET6)
                {
                    var ptr = (MIB_TCP6ROW_OWNER_PID*) (b + 4);

                    for (var i = 0; i < count; i++)
                    {
                        var pid = ptr[i].dwOwningPid;
                        if (!stats.TryGetValue(pid, out var stat))
                            stats[pid] = stat = new TcpStateStatistics();
                        Add(stat, ptr[i].dwState);
                    }
                }
            }
        }

        private static unsafe int EstimateBufferSize(AddressFamily addressFamily, TCP_TABLE_CLASS tableClass)
        {
            var size = 0;
            var status = IPHlpApi.GetExtendedTcpTable(null, ref size, false, addressFamily, tableClass, 0);
            if (status != (int) ErrorCodes.ERROR_INSUFFICIENT_BUFFER)
                Win32ExceptionUtility.Throw(status);
            return size;
        }

        private static void AddToStats(ref TcpStateStatistics stats, MIB_TCP_STATE tcpState, int pid, int rawPort, int? pidFilter, int? portFilter)
        { 
            if (pidFilter.HasValue && pid != pidFilter)
                return;
            var port = GetPort(rawPort);
            if (portFilter.HasValue && port != portFilter)
                return;
            Add(stats, tcpState);
        }

        private static int GetPort(int portInNetworkOrder) =>
            (ushort) IPAddress.NetworkToHostOrder((short) portInNetworkOrder);

        private static void Add(TcpStateStatistics stats, MIB_TCP_STATE state)
        {
            switch (state)
            {
                case MIB_TCP_STATE.MIB_TCP_STATE_CLOSED:
                    stats.Closed++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_LISTEN:
                    stats.Listen++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_SYN_SENT:
                    stats.SynSent++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_SYN_RCVD:
                    stats.SynRcvd++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_ESTAB:
                    stats.Established++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_FIN_WAIT1:
                    stats.FinWait1++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_FIN_WAIT2:
                    stats.FinWait2++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_CLOSE_WAIT:
                    stats.CloseWait++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_CLOSING:
                    stats.Closing++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_LAST_ACK:
                    stats.LastAck++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_TIME_WAIT:
                    stats.TimeWait++;
                    break;
                case MIB_TCP_STATE.MIB_TCP_STATE_DELETE_TCB:
                    stats.DeleteTcb++;
                    break;
                default:
                    stats.Unknown++;
                    break;
            }
        }
    }
}