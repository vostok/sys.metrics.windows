using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct KernelMemoryInfo
    {
        public long PagedPoolBytes;
        public long NonpagedPoolBytes;

        public long TotalBytes => PagedPoolBytes + NonpagedPoolBytes;
        
        public override string ToString()
        {
            return $"PagedPool = {new DataSize(PagedPoolBytes)}; NonPagedPool = {new DataSize(NonpagedPoolBytes)}";
        }
    }
}