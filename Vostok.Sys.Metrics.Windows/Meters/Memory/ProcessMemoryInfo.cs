using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct ProcessMemoryInfo
    {
        public long PrivateBytes { get; set; }
        public long WorkingSetBytes { get; set; }

        public override string ToString()
        {
            return $"{nameof(WorkingSetBytes)}: {new DataSize(WorkingSetBytes)}, {nameof(PrivateBytes)}: {new DataSize(PrivateBytes)}";
        }
    }
}