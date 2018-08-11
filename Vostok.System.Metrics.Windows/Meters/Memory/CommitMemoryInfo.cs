using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Memory
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