using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct CacheMemoryInfo
    {
        public StandbyCacheMemoryInfo Standby;
        public long FileCacheBytes;

        public override string ToString()
        {
            return $"FileCache: {new DataSize(FileCacheBytes)}; {Standby}";
        }
    }
}