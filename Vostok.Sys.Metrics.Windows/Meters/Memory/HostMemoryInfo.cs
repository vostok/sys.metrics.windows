using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct HostMemoryInfo
    {
        public long TotalPhysicalMemoryBytes;
        public long AvailablePhysicalMemoryBytes;

        public override string ToString()
            =>
                $"Used memory: {new DataSize(TotalPhysicalMemoryBytes - AvailablePhysicalMemoryBytes)}/{new DataSize(TotalPhysicalMemoryBytes)} ({new DataSize(AvailablePhysicalMemoryBytes)} available)";
    }
}