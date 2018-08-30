using System.Collections.Generic;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters
{
    internal class InstancesCounter
    {
        private readonly Dictionary<string, int> instanceIndexes = new Dictionary<string, int>();

        public int GetAndIncrement(string name)
        {
            var value = instanceIndexes.TryGetValue(name, out var v) ? v : 0;
            instanceIndexes[name] = value + 1;
            return value;
        }

        public void Clear() => instanceIndexes.Clear();
    }
}