using System.Linq;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch
{
    internal class BatchPerformanceCounter<T> : IPerformanceCounter<T> where T : new ()
    {
        private readonly string instanceName;
        private readonly PdhCounterInfo<T>[] counters;
        private readonly CounterContext<T> context = new CounterContext<T>();
        private PdhQuery query;
        private bool firstObservation = true;

        public BatchPerformanceCounter(CounterInfo<T>[] counters, string instanceName = null)
        {
            this.counters = counters
                .Select(x => new PdhCounterInfo<T> {Info = x})
                .ToArray();
            this.instanceName = instanceName;
        }

        public T Observe()
        {
            if (query == null || query.IsInvalid)
                Init();

            Collect();

            try
            {
                context.Result = Factory.Create<T>();
                for (var i = 0; i < counters.Length; ++i)
                {
                    ref var counter = ref counters[i];
                    counter.Info.SetValue(context, GetValue(counter.PdhCounter, firstObservation));
                }

                return context.Result;
            }
            finally
            {
                firstObservation = false;
            }
        }

        private void Init()
        {
            query = PdhUtilities.OpenRealtimeQuery();
            for (var i = 0; i < counters.Length; ++i)
            {
                ref var counter = ref counters[i];
                var counterPath = instanceName == null
                    ? CounterPathFactory.Create(counter.Info.Name.CategoryName, counter.Info.Name.CounterName)
                    : CounterPathFactory.Create(counter.Info.Name.CategoryName, counter.Info.Name.CounterName, instanceName);
                counter.PdhCounter = query.AddCounter(counterPath);
            }
        }

        private void Collect()
        {
            var status = query.CollectQueryData();
            if (status == PdhStatus.PDH_NO_DATA)
                throw new InvalidInstanceException(instanceName);
            status.EnsureSuccess(nameof(PdhUtilities.CollectQueryData));
        }

        private double GetValue(PdhCounter counter, bool firstObservation)
        {
            var status = counter.GetFormattedValue(out var counterValue);
            if (status == PdhStatus.PDH_INVALID_DATA)
            {
                if (counterValue.CStatus == PdhStatus.PDH_CSTATUS_NO_INSTANCE)
                    throw new InvalidInstanceException(instanceName);
                if (firstObservation)
                    return 0;
            }

            status.EnsureSuccess(nameof(PdhUtilities.GetFormattedValue));
            return counterValue.DoubleValue;
        }

        public void Dispose()
            => query.Dispose();
    }
}