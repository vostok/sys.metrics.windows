using System.Management;
using BenchmarkDotNet.Attributes;
using Vostok.Sys.Metrics.Windows.Native.Constants;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;

namespace Vostok.Sys.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class PhysicalCoresBenchmark
    {
        [Benchmark]
        public unsafe int WinApi()
        {
            var length = 0;
            Kernel32.GetLogicalProcessorInformationEx(LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorCore, null, ref length);
            return length / sizeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX);
        }

        [Benchmark]
        public int WMI()
        {
            var count = 0;
            
            using (var searcher = new ManagementObjectSearcher("Select NumberOfCores from Win32_Processor"))
            using (var collection = searcher.Get())
                foreach (var managementBaseObject in collection)
                    using (managementBaseObject)
                        count += int.Parse(managementBaseObject["NumberOfCores"].ToString());
            
            return count;
        }
    }
}