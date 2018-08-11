using System;
using System.Runtime.InteropServices;
using Vostok.System.Metrics.Windows.Native.Constants;
using Vostok.System.Metrics.Windows.Native.Libraries;
using Vostok.System.Metrics.Windows.Native.Structures;

namespace Vostok.System.Metrics.Windows.Meters.CPU
{
    /// <summary>
    /// Provides information about CPUs on a local machine.
    /// </summary>
    public static class ProcessorInfo
    {
        /// <summary>
        /// Logical cores count (with HT). 
        /// It is a cached value of <see cref="Environment.ProcessorCount"/>
        /// </summary>
        public static readonly int LogicalCores = Environment.ProcessorCount;

        /// <summary>
        /// Physical cores count, as reported by kernel32!GetLogicalProcessorInformationEx or Win32_Processor.NumberOfCores management object.
        /// If management object fails <see cref="PhysicalCores"/> defaults to <see cref="LogicalCores"/>
        /// </summary>
        public static readonly int PhysicalCores;

        static ProcessorInfo()
        {
            if (TryCountPhysicalCoresWithWinApi(out PhysicalCores))
                return;
            PhysicalCores = LogicalCores;
        }

        private static unsafe bool TryCountPhysicalCoresWithWinApi(out int count)
        {
            count = 0;
            var length = 0;
            try
            {
                if (Kernel32.GetLogicalProcessorInformationEx(LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorCore,
                    null, ref length))
                    return false;
                if (Marshal.GetLastWin32Error() != (int) ErrorCodes.ERROR_INSUFFICIENT_BUFFER)
                    return false;
                count = length / sizeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX);
                return count > 0;
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e);
                return false;
            }
        }
    }
}