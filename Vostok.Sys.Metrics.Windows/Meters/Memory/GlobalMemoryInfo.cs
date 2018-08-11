namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public class GlobalMemoryInfo
    {
        public CommitMemoryInfo Commit;
        public KernelMemoryInfo Kernel;
        public CacheMemoryInfo Cache;

        public override string ToString()
        {
            return $"{Commit}; {Kernel}; {Cache}";
        }
    }
}