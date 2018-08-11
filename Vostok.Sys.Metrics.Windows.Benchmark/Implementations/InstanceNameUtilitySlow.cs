using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations
{
    internal class InstanceNameUtilitySlow
    {
        private readonly string categoryName;
        private readonly string counterName;
        public static readonly InstanceNameUtilitySlow Global = new InstanceNameUtilitySlow("Process", "id process");
        public static readonly InstanceNameUtilitySlow DotNet = new InstanceNameUtilitySlow(".NET CLR Memory", "process id");

        private InstanceNameUtilitySlow(string categoryName, string counterName)
        {
            this.categoryName = categoryName;
            this.counterName = counterName;
        }

        public Dictionary<int, string> ObtainInstanceNames()
        {
            var category = new PerformanceCounterCategory(this.categoryName);

            var dataCollectionByCounters = category.ReadCategory();
            var dataCollection = dataCollectionByCounters[counterName];
            var map = new Dictionary<int, string>(dataCollection.Count);
            
            foreach (DictionaryEntry kvp in dataCollection)
            {
                var instanceId = (string) kvp.Key;
                var value = (InstanceData) kvp.Value;
                var pid = (int) value.RawValue;
                map[pid] = instanceId;
            }

            return map;
        }
    }
}