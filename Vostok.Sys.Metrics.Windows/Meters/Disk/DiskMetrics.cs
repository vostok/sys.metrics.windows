using System;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
{
    public class DiskMetrics
    {
        /// <summary>
        /// Current disk queue length
        /// </summary>
        public int QueueLength { get; internal set; }
        public long ReadBytesPerSecond { get; internal set; }
        public long WriteBytesPerSecond { get; internal set; }
        public int ReadsPerSecond { get; internal set; }
        public int WritesPerSecond { get; internal set; }
        public TimeSpan ReadLatency { get; internal set; }
        public TimeSpan WriteLatency { get; internal set; }
        public double Load { get; internal set; }
        public string Drive { get; internal set; }
        public long FreeSpaceBytes { get; internal set; }

        public override string ToString()
        {   
            return $"Disk {Drive} usage: {(Load*100).Format()}%; Queue: {QueueLength}; Free space: {new DataSize(FreeSpaceBytes)}; " +
                   $"Read: {new DataSize(ReadBytesPerSecond)}/s, {ReadsPerSecond} op/s; Write: {new DataSize(WriteBytesPerSecond)}/s, {WritesPerSecond} op/s; " +
                   $"Latency - read: {(int) ReadLatency.TotalMilliseconds} ms, write: {(int) WriteLatency.TotalMilliseconds} ms";
        }
    }
}