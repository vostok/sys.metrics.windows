using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct CacheMemoryInfo
    {
        public StandbyCacheMemoryInfo Standby;
        public DataSize File;

        public override string ToString()
        {
            return $"FileCache: {File}; {Standby}";
        }
    }
}