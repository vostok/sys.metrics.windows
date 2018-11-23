using System;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    /// <summary>
    /// <para>
    /// Provides information about TCP connection states machine-wide.
    /// </para>
    /// 
    /// <para>
    /// Internally uses IpHlpAPI GetExtendedTcpTable function
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa365928(v=vs.85).aspx
    /// </para>
    /// </summary>
    public class TcpConnectionsMeter : IDisposable
    {
        //TODO: use single class with many methods for tcp stats?
        
        private readonly int? pid;
        private readonly int? localPort;

        protected TcpConnectionsMeter(int? pid, int? localPort)
        {
            this.pid = pid;
            this.localPort = localPort;
        }

        /// <summary>
        /// Creates machine-wide <see cref="TcpConnectionsMeter"/>
        /// </summary>
        public TcpConnectionsMeter() : this(null, null) { }
        
        public TcpStateStatistics GetStatistics()
        {
            var stats = new TcpStateStatistics();
            GetStatisticsInternal(AddressFamily.AF_INET, stats);
            GetStatisticsInternal(AddressFamily.AF_INET6, stats);
            return stats;
        }
        
        public TcpStateStatistics GetStatisticsByTCPv4()
        {
            var stats = new TcpStateStatistics();
            GetStatisticsInternal(AddressFamily.AF_INET, stats);
            return stats;
        }
        
        public TcpStateStatistics GetStatisticsByTCPv6()
        {
            var stats = new TcpStateStatistics();
            GetStatisticsInternal(AddressFamily.AF_INET6, stats);
            return stats;
        }
        
        private void GetStatisticsInternal(AddressFamily addressFamily, TcpStateStatistics stats)
            => NetworkUtility.GetTcpStats(addressFamily, stats, pid, localPort);

        public void Dispose()
        {
        }
    }

    /// <summary>
    /// <para>
    /// Provides information about TCP connection states by process.
    /// </para>
    /// 
    /// <para>
    /// Internally uses IpHlpAPI GetExtendedTcpTable function
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa365928(v=vs.85).aspx
    /// </para>
    /// </summary>
    public class ProcessTcpConnectionsMeter : TcpConnectionsMeter
    {
        /// <summary>
        /// Creates <see cref="PortTcpConnectionsMeter"/> for current process
        /// </summary>
        public ProcessTcpConnectionsMeter() : base(ProcessUtility.CurrentProcessId, null) { }
        
        /// <summary>
        /// Creates <see cref="ProcessTcpConnectionsMeter"/> for process with given pid
        /// </summary>
        /// <param name="pid"></param>
        public ProcessTcpConnectionsMeter(int pid) : base(pid, null) { }
    }

    /// <summary>
    /// <para>
    /// Provides information about TCP connection states by local port. Useful for server applications
    /// </para>
    /// 
    /// <para>
    /// Internally uses IpHlpAPI GetExtendedTcpTable function
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa365928(v=vs.85).aspx
    /// </para>
    /// </summary>
    public class PortTcpConnectionsMeter : TcpConnectionsMeter
    {
        /// <summary>
        /// Creates <see cref="PortTcpConnectionsMeter"/> for target port
        /// </summary>
        /// <param name="localPort"></param>
        public PortTcpConnectionsMeter(int localPort) : base(null, localPort) { }
        public PortTcpConnectionsMeter(int localPort, int pid) : base(pid, localPort) { }
    }
}