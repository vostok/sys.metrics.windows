using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Memory
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