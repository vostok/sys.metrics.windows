using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Memory
{
    public struct StandbyCacheMemoryInfo
    {        
        public DataSize NormalPriority;
        public DataSize Reserve;

        public DataSize Total => NormalPriority + Reserve;
        
        public override string ToString()
        {
            return $"Standby: NormalPriority = {NormalPriority}; Reserve = {Reserve}";
        }
    }
}