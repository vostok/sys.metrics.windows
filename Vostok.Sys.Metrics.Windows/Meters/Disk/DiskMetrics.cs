using System;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
{
    public class DiskMetrics
    {
        /// <summary>
        /// Current disk queue length
        /// </summary>
        public int QueueLength;
        public DataSize ReadBytesPerSecond;
        public DataSize WriteBytesPerSecond;
        public int ReadsPerSecond;
        public int WritesPerSecond;
        public TimeSpan ReadLatency;
        public TimeSpan WriteLatency;
        public double Load;
        public string Drive;
        public DataSize FreeSpace;

        public override string ToString()
        {   
            return $"Disk {Drive} usage: {(Load*100).Format()}%; Queue: {QueueLength}; Free space: {FreeSpace}; " +
                   $"Read: {ReadBytesPerSecond}/s, {ReadsPerSecond} op/s; Write: {WriteBytesPerSecond}/s, {WritesPerSecond} op/s; " +
                   $"Latency - read: {(int) ReadLatency.TotalMilliseconds} ms, write: {(int) WriteLatency.TotalMilliseconds} ms";
        }
    }
}