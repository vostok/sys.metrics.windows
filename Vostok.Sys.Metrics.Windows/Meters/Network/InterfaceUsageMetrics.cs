using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    public struct InterfaceUsageMetrics
    {
        /// <summary>
        /// Interface name
        /// </summary>
        public string Interface { get; set; }
        
        /// <summary>
        /// Bytes received / second
        /// </summary>
        public long ReceivedBytesPerSecond { get; set; }
        
        /// <summary>
        /// Bytes sent / second
        /// </summary>
        public long SentBytesPerSecond { get; set; }

        /// <summary>
        /// Interface bandwidth
        /// </summary>
        public long BandwidthBytes { get; set; }

        public override string ToString()
            => $"{Interface} - In: {new DataSize(ReceivedBytesPerSecond)}/s, Out: {new DataSize(SentBytesPerSecond)}/s, Bandwidth: {new DataSize(BandwidthBytes)}/s ({new DataSize(BandwidthBytes).ToStringAsDecimalBits()}/s)";
    }
}