using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct CommitMemoryInfo
    {
        public DataSize Committed;
        public DataSize Limit;

        public double Usage => Committed / Limit;
        
        public override string ToString()
        {
            return $"Committed = {Committed}/{Limit}";
        }
    }
}