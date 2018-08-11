using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Vostok.Sys.Metrics.Windows.Benchmark.Implementations;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;
using Vostok.Sys.Metrics.Windows.TestProcess;

namespace Vostok.Sys.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class InstanceNameUtilityBenchmark
    {
        [Params(10, 100, 500)]
        public int AdditionalProcessCount { get; set; }
        private List<TestProcessHandle> processes;

        [GlobalSetup]
        public void Setup()
        {
            processes = new List<TestProcessHandle>();
            for (int i = 0; i < AdditionalProcessCount; i++)
            {
                var testProcess = new TestProcessHandle();
                processes.Add(testProcess);
                testProcess.MakeGC(0);
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            processes.ForEach(p => p.Dispose());
        }
        
        [Benchmark]
        public void ObtainSlow()
            => InstanceNameUtilitySlow.Global.ObtainInstanceNames();
        
        [Benchmark]
        public void ObtainSlowNet()
            => InstanceNameUtilitySlow.DotNet.ObtainInstanceNames();
        
        [Benchmark]
        public void ObtainPdh()
            => InstanceNameUtility.AllProcesses.ObtainInstanceNames();
        
        [Benchmark]
        public void ObtainPdhNet()
            => InstanceNameUtility.NetProcesses.ObtainInstanceNames();
    }
}