using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using Vostok.System.Metrics.Windows.TestProcess;
using NUnit.Framework;
using Vostok.System.Metrics.Windows.PerformanceCounters;
using Vostok.System.Metrics.Windows.TestProcess;

namespace Vostok.System.Metrics.Windows.IntegrationTests
{
    internal class InstanceNameUtility_Tests
    {
        [TestCase(true, ExpectedResult = true, TestName = "After GC")]
        [TestCase(false, ExpectedResult = false, TestName = "Before GC")]
        public bool DotNet_process_appears_in_DotNet_cache_only_after_gc(bool makeGc)
        {
            using (var testProcess = new TestProcessHandle(false))
            {
                if (makeGc)
                    testProcess.MakeGC(0);
                
                var cache = InstanceNameUtility.NetProcesses.ObtainInstanceNames();
                return cache.ContainsKey(testProcess.Process.Id);
            }
        }

        [Test]
        public void DotNetCache_does_not_contains_non_DotNet_processes()
        {
            var cache = InstanceNameUtility.NetProcesses.ObtainInstanceNames();
            var pid = Process.GetProcessesByName("svchost")[0].Id;
            cache.ContainsKey(pid).Should().BeFalse();
        }
        
        [Test]
        public void Instance_names_includes_sequential_numbers_in_global_cache()
        {
            using (var testProcess0 = new TestProcessHandle())
            using (var testProcess1 = new TestProcessHandle())
            using (var testProcess2 = new TestProcessHandle())
            using (var testProcess3 = new TestProcessHandle())
            {
                var cache = InstanceNameUtility.AllProcesses.ObtainInstanceNames();
                cache[testProcess0.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess");
                cache[testProcess1.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#1");
                cache[testProcess2.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#2");
                cache[testProcess3.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#3");
            }
        }
        
        [Test]
        public void Instance_names_includes_reversed_sequential_numbers_in_net_cache()
        {
            using (var testProcess0 = new TestProcessHandle())
            using (var testProcess1 = new TestProcessHandle())
            using (var testProcess2 = new TestProcessHandle())
            using (var testProcess3 = new TestProcessHandle())
            {
                var cache = InstanceNameUtility.NetProcesses.ObtainInstanceNames();

                cache[testProcess0.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#3");
                cache[testProcess1.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#2");
                cache[testProcess2.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#1");
                cache[testProcess3.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess");
            }
        }
        [Test]
        public void Instance_names_for_net_cache_should_not_change_after_gc()
        {
            using (var testProcess0 = new TestProcessHandle())
            using (var testProcess1 = new TestProcessHandle())
            {
                void AssertExpectedInstanceIds(Dictionary<int, string> cache)
                {
                    cache[testProcess0.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#1");
                    cache[testProcess1.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess");
                }
                
                AssertExpectedInstanceIds(InstanceNameUtility.NetProcesses.ObtainInstanceNames());
                
                testProcess0.MakeGC(0);
                AssertExpectedInstanceIds(InstanceNameUtility.NetProcesses.ObtainInstanceNames());    
                
                testProcess1.MakeGC(0);
                AssertExpectedInstanceIds(InstanceNameUtility.NetProcesses.ObtainInstanceNames());    
            }
        }
        
        [Test]
        public void Instance_names_should_change_after_process_with_less_instance_id_exit_in_global_cache()
        {
            using (var testProcess0 = new TestProcessHandle())
            using (var testProcess1 = new TestProcessHandle())
            {
                var cacheBefore = InstanceNameUtility.AllProcesses.ObtainInstanceNames();
                cacheBefore[testProcess1.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#1");
                
                testProcess0.Dispose();
                
                var cacheAfter = InstanceNameUtility.AllProcesses.ObtainInstanceNames();
                cacheAfter[testProcess1.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess");
                
            }
        }
        
        [Test]
        public void Instance_names_should_change_after_process_with_less_instance_id_exit_in_net_cache()
        {
            using (var testProcess0 = new TestProcessHandle())
            using (var testProcess1 = new TestProcessHandle())
            {
                var cacheBefore = InstanceNameUtility.NetProcesses.ObtainInstanceNames();
                cacheBefore[testProcess0.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess#1");
                
                testProcess1.Dispose();
                
                var cacheAfter = InstanceNameUtility.NetProcesses.ObtainInstanceNames();
                cacheAfter[testProcess0.Process.Id].Should().BeEquivalentTo("vostok.system.metrics.windows.testprocess");
            }
        }
        
        [Test]
        public void Instance_names_in_global_and_net_cache_are_not_equal()
        {
            using (var testProcess0 = new TestProcessHandle())
            using (var testProcess1 = new TestProcessHandle())
            {
                var pid0 = testProcess0.Process.Id;
                var pid1 = testProcess1.Process.Id;
                
                var globalCache = InstanceNameUtility.AllProcesses.ObtainInstanceNames();
                var netCache = InstanceNameUtility.NetProcesses.ObtainInstanceNames();

                Console.WriteLine($"testProcess0 - global: {globalCache[pid0]}, net: {netCache[pid0]}");
                Console.WriteLine($"testProcess1 - global: {globalCache[pid1]}, net: {netCache[pid1]}");

                globalCache[pid0].Should().NotBe(netCache[pid0]);
                globalCache[pid1].Should().NotBe(netCache[pid1]);
            }
        }
    }
}