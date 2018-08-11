using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Network
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
        public DataSize ReceivedPerSecond;
        
        /// <summary>
        /// Bytes sent / second
        /// </summary>
        public DataSize SentPerSecond;

        /// <summary>
        /// Interface bandwidth
        /// </summary>
        public DataSize Bandwidth;

        public override string ToString()
            => $"{Interface} - In: {ReceivedPerSecond}/s, Out: {SentPerSecond}/s, Bandwidth: {Bandwidth}/s ({Bandwidth.ToStringAsDecimalBits()}/s)";
    }
}