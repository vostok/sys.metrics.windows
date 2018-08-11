using System;
using System.Runtime.InteropServices;
using Vostok.Sys.Metrics.Windows.Native.Structures;

namespace Vostok.Sys.Metrics.Windows.Native.Libraries
{
    internal static class Pdh
    {
        private const string pdh = "pdh.dll";

        [DllImport(pdh)]
        public static extern PdhStatus PdhOpenQuery(string dataSource, IntPtr userData, out PdhQuery query);
        
        [DllImport(pdh)]
        public static extern PdhStatus PdhCloseQuery(IntPtr query);

        [DllImport(pdh)]
        public static extern PdhStatus PdhAddCounter(PdhQuery query, string path, IntPtr userData,
            out PdhCounter counter);

        [DllImport(pdh)]
        public static extern PdhStatus PdhAddEnglishCounter(PdhQuery query, string path, IntPtr userData,
            out PdhCounter counter);

        [DllImport(pdh)]
        public static extern PdhStatus PdhCollectQueryData(PdhQuery query);

        [DllImport(pdh)]
        public static extern unsafe PdhStatus PdhGetRawCounterArray(PdhCounter counter, ref int bufferSize,
            out int itemCount, PDH_RAW_COUNTER_ITEM* buffer);

        [DllImport(pdh)]
        public static extern PdhStatus PdhGetFormattedCounterValue(PdhCounter counter, PdhFmt dwFormat,
            out int lpdwType, out PDH_FMT_COUNTERVALUE pValue);
        
        [DllImport(pdh)]
        public static extern unsafe PdhStatus PdhGetFormattedCounterArray(
            PdhCounter counter,
            PdhFmt dwFormat,
            ref int bufferSize,
            out int itemCount, PDH_FMT_COUNTERVALUE_ITEM* buffer);

        [DllImport(pdh)]
        public static extern PdhStatus PdhGetRawCounterValue(PdhCounter counter, out int lpdwType,
            out PDH_RAW_COUNTER pValue);

        [DllImport(pdh)]
        public static extern PdhStatus PdhCalculateCounterFromRawValue(
            PdhCounter hCounter, PdhFmt dwFormat,
            ref PDH_RAW_COUNTER rawValue1,
            ref PDH_RAW_COUNTER rawValue2,
            out PDH_FMT_COUNTERVALUE fmtValue);
        
    }
}