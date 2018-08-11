using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Memory
{
    public struct KernelMemoryInfo
    {
        public DataSize PagedPool;
        public DataSize NonpagedPool;

        public DataSize Total => PagedPool + NonpagedPool;
        
        public override string ToString()
        {
            return $"PagedPool = {PagedPool}; NonPagedPool = {NonpagedPool}";
        }
    }
}