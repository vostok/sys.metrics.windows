using System;
using System.Collections.Generic;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    /// <summary>
    /// <para>
    /// Provides information about TCP connection states by all processes separately.
    /// </para>
    /// 
    /// <para>
    /// Internally uses IpHlpAPI GetExtendedTcpTable function
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa365928(v=vs.85).aspx
    /// </para>
    /// </summary>
    public class AllProcessesTcpConnectionsMeter : IDisposable
    {
        //TODO: rename
        
        public TcpStateStatisticsByProcesses GetStatistics()
        {
            var stat = new Dictionary<int, TcpStateStatistics>();
            NetworkUtility.GetAllTcpStats(AddressFamily.AF_INET, stat);
            NetworkUtility.GetAllTcpStats(AddressFamily.AF_INET6, stat);
            return new TcpStateStatisticsByProcesses(stat);
        }

        public TcpStateStatisticsByProcesses GetStatisticsByTCPv4()
        {
            var stat = new Dictionary<int, TcpStateStatistics>();
            NetworkUtility.GetAllTcpStats(AddressFamily.AF_INET, stat);
            return new TcpStateStatisticsByProcesses(stat);   
        }

        public TcpStateStatisticsByProcesses GetStatisticsByTCPv6()
        {
            var stat = new Dictionary<int, TcpStateStatistics>();
            NetworkUtility.GetAllTcpStats(AddressFamily.AF_INET6, stat);
            return new TcpStateStatisticsByProcesses(stat);
        }
        
        public void Dispose()
        {
        }
    }
}