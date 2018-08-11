using System;
using System.Diagnostics;
using FluentAssertions;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.Meters.Memory;
using Vostok.System.Metrics.Windows.TestProcess;
using NUnit.Framework;

namespace Vostok.System.Metrics.Windows.IntegrationTests
{
    [TestFixture]
    public class ProcessMemoryMeter_Tests
    {
        [Test]
        public void Process_memory_meter_works()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ProcessMemoryMeter(testProcess.Process.Id))
            {
                var toEat = DataSize.FromMegabytes(100);
                var tolerance = DataSize.FromMegabytes(2);

                var before = meter.GetMemoryInfo();
                testProcess.EatMemory(toEat);
                var after = meter.GetMemoryInfo();

                Console.WriteLine($"Before: ws {before.WorkingSet} pb {before.Private}");
                Console.WriteLine($"After: ws {after.WorkingSet} pb {after.Private}");
                Console.WriteLine($"Diff ws {after.WorkingSet - before.WorkingSet} pb {after.Private - before.Private}");

                (after.WorkingSet - before.WorkingSet).Should().BeGreaterThan(toEat - tolerance);
                (after.Private - before.Private).Should().BeGreaterThan(toEat - tolerance);
            }
        }
        
        [Test]
        public void Process_memory_meter_works_for_private_memory()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ProcessMemoryMeter(testProcess.Process.Id))
            {
                var toEat = DataSize.FromMegabytes(100);
                var tolerance = DataSize.FromMegabytes(2);

                var before = meter.GetMemoryInfo();
                testProcess.EatPrivateMemory(toEat);
                var after = meter.GetMemoryInfo();

                Console.WriteLine($"Before: ws {before.WorkingSet} pb {before.Private}");
                Console.WriteLine($"After: ws {after.WorkingSet} pb {after.Private}");
                Console.WriteLine($"Diff ws {after.WorkingSet - before.WorkingSet} pb {after.Private - before.Private}");

                (after.WorkingSet - before.WorkingSet).Should().BeLessThan(tolerance);
                (after.Private - before.Private).Should().BeGreaterThan(toEat - tolerance);
            }
        }
        
        [Test]
        public void Process_memory_meter_fails_after_process_exit()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ProcessMemoryMeter(testProcess.Process.Id))
            {
                testProcess.Dispose();
                new Action(() => meter.GetMemoryInfo()).Should().Throw<InvalidOperationException>();
            }
        }
        
        [Test]
        public void Process_memory_meter_should_not_throw_in_ctor_when_process_doesnt_exists()
        {
            var pid = GetNonExistentProcessId();
            new Action(() => new ProcessMemoryMeter(pid)).Should().NotThrow();
        }

        [Test]
        public void Does_not_throw()
        {
            using (var testProcess = new TestProcessHandle())
            using (var memMeter = new ProcessMemoryMeter(testProcess.Process.Id))
            {
                var memInfo = memMeter.GetMemoryInfo();
                Console.WriteLine(memInfo);
            }
        }

        [Test]
        public void Process_memory_meter_should_throw_on_first_call_when_process_doesnt_exists()
        {
            var pid = GetNonExistentProcessId();
            var meter = new ProcessMemoryMeter(pid);
            new Action(() => meter.GetMemoryInfo()).Should().ThrowExactly<InvalidOperationException>();
        }

        private static int GetNonExistentProcessId()
        {
            var pid = 100000;
            while (true)
            {
                try
                {
                    Process.GetProcessById(pid);
                }
                catch (ArgumentException)
                {
                    return pid;
                }

                pid += 4;
            }
        }
    }
}