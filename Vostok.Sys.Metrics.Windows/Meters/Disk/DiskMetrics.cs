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
        public long ReadBytesPerSecondBytes;
        public long WriteBytesPerSecondBytes;
        public int ReadsPerSecond;
        public int WritesPerSecond;
        public TimeSpan ReadLatency;
        public TimeSpan WriteLatency;
        public double Load;
        public string Drive;
        public long FreeSpaceBytes;

        public override string ToString()
        {   
            return $"Disk {Drive} usage: {(Load*100).Format()}%; Queue: {QueueLength}; Free space: {new DataSize(FreeSpaceBytes)}; " +
                   $"Read: {new DataSize(ReadBytesPerSecondBytes)}/s, {ReadsPerSecond} op/s; Write: {new DataSize(WriteBytesPerSecondBytes)}/s, {WritesPerSecond} op/s; " +
                   $"Latency - read: {(int) ReadLatency.TotalMilliseconds} ms, write: {(int) WriteLatency.TotalMilliseconds} ms";
        }
    }
}