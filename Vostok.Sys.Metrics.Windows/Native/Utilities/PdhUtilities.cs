using System;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;

namespace Vostok.Sys.Metrics.Windows.Native.Utilities
{
    internal static class PdhUtilities
    {
        private const PdhFmt DefaultFmt = PdhFmt.PDH_FMT_DOUBLE | PdhFmt.PDH_FMT_NOCAP100;
        
        public static PdhCounter AddCounter(this PdhQuery query, string path)
        {
            Pdh
                .PdhAddEnglishCounter(query, path, IntPtr.Zero, out var counter)
                .EnsureSuccess(nameof(Pdh.PdhAddEnglishCounter));

            return counter;
        }

        public static PdhStatus CollectQueryData(this PdhQuery query) => Pdh.PdhCollectQueryData(query);

        public static PdhStatus GetFormattedValue(this PdhCounter counter, out PDH_FMT_COUNTERVALUE value)
            => Pdh.PdhGetFormattedCounterValue(
                counter,
                DefaultFmt,
                out _,
                out value);

        public static bool IsLargerBufferRequired(this PdhStatus status)
            => status == PdhStatus.PDH_MORE_DATA;
        
        public static void EnsureSuccess(this PdhStatus status, string method)
        {
            if (status != 0)
                FailWithError(status, method);
        }
        
        public static void EnsureStatus(this PdhStatus status, PdhStatus successfulStatus, string method)
        {
            if (status != successfulStatus)
                FailWithError(status, method);
        }

        public static unsafe PdhStatus GetRawCounterArray(this PdhCounter counter, ref int bufferSize,
            out int itemCount, PDH_RAW_COUNTER_ITEM* buffer) =>
            Pdh.PdhGetRawCounterArray(counter, ref bufferSize, out itemCount, buffer);

        public static unsafe PdhStatus GetFormattedCounterArray(this PdhCounter counter, ref int bufferSize,
            out int itemCount, PDH_FMT_COUNTERVALUE_ITEM* buffer) =>
            Pdh.PdhGetFormattedCounterArray(counter, DefaultFmt, ref bufferSize, out itemCount, buffer);

        public static unsafe int EstimateRawCounterArraySize(this PdhCounter counter)
        {
            var size = 0;
            Pdh.PdhGetRawCounterArray(counter, ref size, out _, null).EnsureStatus(PdhStatus.PDH_MORE_DATA, nameof(Pdh.PdhGetRawCounterArray));
            return size;
        }

        public static unsafe int EstimateFormattedCounterArraySize(this PdhCounter counter)
        {
            var size = 0;
            Pdh.PdhGetFormattedCounterArray(counter, DefaultFmt, ref size, out _, null).EnsureStatus(PdhStatus.PDH_MORE_DATA, nameof(Pdh.PdhGetRawCounterArray));
            return size;
        }

        public static PdhQuery OpenRealtimeQuery()
        {
            Pdh.PdhOpenQuery(null, IntPtr.Zero, out var pdhQuery).EnsureSuccess(nameof(Pdh.PdhOpenQuery));
            return pdhQuery;
        }

        private static void FailWithError(PdhStatus error, string function)
            => throw CreateException(error, function);

        private static InvalidOperationException CreateException(PdhStatus error, string function)
            => new InvalidOperationException(string.Format("Pdh function {0} failed with code {1:x8} ({2})", function,
                (int) error, Enum.GetName(typeof(PdhStatus), error)));
    }
}