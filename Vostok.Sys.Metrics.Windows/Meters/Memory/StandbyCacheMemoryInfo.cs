using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct StandbyCacheMemoryInfo
    {        
        public long NormalPriorityBytes;
        public long ReserveBytes;

        public long TotalBytes => NormalPriorityBytes + ReserveBytes;
        
        public override string ToString()
        {
            return $"Standby: NormalPriority = {new DataSize(NormalPriorityBytes)}; Reserve = {new DataSize(ReserveBytes)}";
        }
    }
}