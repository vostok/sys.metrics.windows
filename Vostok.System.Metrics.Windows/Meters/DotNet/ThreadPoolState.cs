namespace Vostok.System.Metrics.Windows.Meters.DotNet
{
    public struct ThreadPoolState
    {
        public readonly int MinWorkerThreads;
        public readonly int UsedWorkerThreads;
        public readonly int MinIocpThreads;
        public readonly int UsedIocpThreads;

        public ThreadPoolState(int minWorkerThreads, int usedWorkerThreads, int minIocpThreads, int usedIocpThreads)
        {
            MinWorkerThreads = minWorkerThreads;
            UsedWorkerThreads = usedWorkerThreads;
            MinIocpThreads = minIocpThreads;
            UsedIocpThreads = usedIocpThreads;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ThreadPoolState))
                return false;
            return Equals((ThreadPoolState)obj);
        }

        public bool Equals(ThreadPoolState other)
        {
            if (MinWorkerThreads == other.MinWorkerThreads && UsedWorkerThreads == other.UsedWorkerThreads && MinIocpThreads == other.MinIocpThreads)
                return UsedIocpThreads == other.UsedIocpThreads;
            return false;
        }

        public override int GetHashCode()
        {
            return ((MinWorkerThreads * 397 ^ UsedWorkerThreads) * 397 ^ MinIocpThreads) * 397 ^ UsedIocpThreads;
        }
    }
}