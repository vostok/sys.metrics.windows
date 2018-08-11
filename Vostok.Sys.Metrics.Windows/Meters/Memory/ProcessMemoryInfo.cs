using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct ProcessMemoryInfo
    {
        public DataSize Private { get; set; }
        public DataSize WorkingSet { get; set; }

        public override string ToString()
        {
            return $"{nameof(WorkingSet)}: {WorkingSet}, {nameof(Private)}: {Private}";
        }
    }
}