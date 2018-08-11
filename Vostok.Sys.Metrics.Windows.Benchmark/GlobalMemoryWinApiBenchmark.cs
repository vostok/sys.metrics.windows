using System;
using BenchmarkDotNet.Attributes;
using Vostok.Sys.Metrics.Windows.Meters.Memory;
using Vostok.Sys.Metrics.Windows.Native.Libraries;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.Sys.Metrics.Windows.Benchmark
{
    public class GlobalMemoryWinApiBenchmark
    {
        [Benchmark]
        public unsafe void GetGlobalMemoryStatusEx()
        {
            var status = new MEMORYSTATUSEX {dwLength = (uint) sizeof(MEMORYSTATUSEX)};
            Kernel32.GlobalMemoryStatusEx(ref status);
        }
        
        [Benchmark]
        public unsafe void PerfCounter()
        {
            meter.GetGlobalMemoryInfo();
        }

        [Benchmark]
#pragma warning disable 618
        public void PerformanceInformation() => PsApi.GetPerformanceInfo(out _);
#pragma warning restore 618
        
        private GlobalMemoryMeter meter = new GlobalMemoryMeter(PerformanceCounterFactory.Default);
    }
}