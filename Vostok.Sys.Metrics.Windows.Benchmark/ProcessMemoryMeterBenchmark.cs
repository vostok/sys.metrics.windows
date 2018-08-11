using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Vostok.Sys.Metrics.Windows.Benchmark.Implementations;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Meters.Memory;
using Vostok.Sys.Metrics.Windows.TestProcess;

namespace Vostok.Sys.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class ProcessMemoryMeterBenchmark
    {
        private TestProcessHandle testProcess;
        private Process currentProcess = Process.GetCurrentProcess();
        private const int N = 50;
        
        [GlobalSetup]
        public void GlobalSetup()
        {
            testProcess = ObtainTestProcess();
        }

        private TestProcessHandle ObtainTestProcess()
        {
            if (testProcess != null)
                return testProcess;
            var p = new TestProcessHandle();
            p.EatMemory(DataSize.FromMegabytes(10).Bytes);
            return p;
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            testProcess.Dispose();
            testProcess = null;
        }
        
        [Benchmark] public void WinApi_NoProcessGiven() => WinApi(null);
        [Benchmark] public void WinApi_CurrentProcess() => WinApi(currentProcess);
        [Benchmark] public void WinApi_OtherProcess() => WinApi(testProcess.Process);
        
        [Benchmark] public void OneTimeHandle_NoProcessGiven() => OneTimeHandle(null);
        [Benchmark] public void OneTimeHandle_CurrentProcess() => OneTimeHandle(currentProcess);
        [Benchmark] public void OneTimeHandle_OtherProcess() => OneTimeHandle(testProcess.Process);
        
        [Benchmark] public void DotNet_NoProcessGiven() => DotNet(null);
        [Benchmark] public void DotNet_CurrentProcess() => DotNet(currentProcess);
        [Benchmark] public void DotNet_OtherProcess() => DotNet(testProcess.Process);
        
        private static void WinApi(Process target)
        {
            using (var meter = new ProcessMemoryMeter(target.Id))
            {
                for(var i = 0; i < N; ++i)
                    meter.GetMemoryInfo();
            }
        }
        
        private static void OneTimeHandle(Process target)
        {
            var pid = target.Id;
            
            using (var meter = new ProcessMemoryMeterWithOneTimeHandle())
            {
                for(var i = 0; i < N; ++i)
                    meter.GetMemoryInfo(pid);
            }
        }
        
        private static void DotNet(Process target)
        {
            using (var meter = new ProcessMemoryMeterSlow(target))
            {
                for(var i = 0; i < N; ++i)
                    meter.GetMemoryInfo();
            }
        }
    }
}