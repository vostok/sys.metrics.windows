using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    public struct InterfaceUsageMetrics
    {
        /// <summary>
        /// Interface name
        /// </summary>
        public string Interface;
        
        /// <summary>
        /// Bytes received / second
        /// </summary>
        public long ReceivedPerSecondBytes;
        
        /// <summary>
        /// Bytes sent / second
        /// </summary>
        public long SentPerSecondBytes;

        /// <summary>
        /// Interface bandwidth
        /// </summary>
        public long BandwidthBytes;

        public override string ToString()
            => $"{Interface} - In: {new DataSize(ReceivedPerSecondBytes)}/s, Out: {new DataSize(SentPerSecondBytes)}/s, Bandwidth: {new DataSize(BandwidthBytes)}/s ({new DataSize(BandwidthBytes).ToStringAsDecimalBits()}/s)";
    }
}