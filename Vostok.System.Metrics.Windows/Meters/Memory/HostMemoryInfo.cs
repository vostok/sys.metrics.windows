using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Memory
{
    public struct HostMemoryInfo
    {
        public DataSize TotalPhysicalMemory;
        public DataSize AvailablePhysicalMemory;

        public override string ToString()
            =>
                $"Used memory: {TotalPhysicalMemory - AvailablePhysicalMemory}/{TotalPhysicalMemory} ({AvailablePhysicalMemory} available)";
    }
}