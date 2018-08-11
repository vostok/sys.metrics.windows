using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Memory
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