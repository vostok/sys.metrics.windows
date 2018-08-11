using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct CommitMemoryInfo
    {
        public long CommittedBytes { get; internal set; }
        public long LimitBytes { get; internal set; }

        public double UsageFraction => CommittedBytes / (double) LimitBytes;
        
        public override string ToString()
        {
            return $"Committed = {new DataSize(CommittedBytes)}/{new DataSize(LimitBytes)}";
        }
    }
}