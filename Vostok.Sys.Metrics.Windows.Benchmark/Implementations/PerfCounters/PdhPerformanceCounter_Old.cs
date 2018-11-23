using System;
using Vostok.Sys.Metrics.PerfCounters;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations.PerfCounters
{
    internal class PdhPerformanceCounter_Old : IPerformanceCounter<double>
    {
        private readonly string instanceName;
        private readonly object syncObject = new object();
        private readonly string path;

        private PdhQuery pdhQuery;
        private PdhCounter pdhCounter;

        public PdhPerformanceCounter_Old(string categoryName, string counterName, string instanceName)
        {
            this.instanceName = instanceName;
            path = CounterPathFactory.Create(categoryName, counterName, instanceName);
        }

        public PdhPerformanceCounter_Old(string categoryName, string counterName)
            => path = CounterPathFactory.Create(categoryName, counterName);

        public double Observe()
        {
            lock (syncObject)
            {
                if (pdhQuery == null || pdhQuery.IsInvalid)
                    Init();
                
                // some counters require two values to calculate formatted value
                // https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa372637(v=vs.85).aspx
                for (var i = 0; i < 2; ++i)
                {
                    Collect();
                    var status = pdhCounter.GetFormattedValue(out var val);
                    if (status == PdhStatus.PDH_INVALID_DATA)
                    {
                        if (val.CStatus == PdhStatus.PDH_CSTATUS_NO_INSTANCE && instanceName != null)
                            throw new InvalidOperationException(instanceName);
                        if (i == 0)
                            continue;
                    }
                    status.EnsureSuccess(nameof(PdhUtilities.GetFormattedValue));
                    return val.DoubleValue;
                }
            }

            return 0; // unreachable
        }

        private void Init()
        {
            Pdh.PdhOpenQuery(null, IntPtr.Zero, out pdhQuery).EnsureSuccess(nameof(Pdh.PdhOpenQuery));
            pdhCounter = pdhQuery.AddCounter(path);
        }
        
        // this code also used in Batch counter, move to Utilities
        private void Collect()
        {
            var status = pdhQuery.CollectQueryData();
            if (status == PdhStatus.PDH_NO_DATA)
                throw new InvalidOperationException(instanceName);
            status.EnsureSuccess(nameof(PdhUtilities.CollectQueryData));
        }
        
        public void Dispose()
            => pdhQuery.Dispose();
    }
}