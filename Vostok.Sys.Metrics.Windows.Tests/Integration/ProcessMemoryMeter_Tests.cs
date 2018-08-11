using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Meters.Memory;
using Vostok.Sys.Metrics.Windows.TestProcess;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
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
                testProcess.EatMemory(toEat.Bytes);
                var after = meter.GetMemoryInfo();

                Console.WriteLine($"Before: ws {before.WorkingSetBytes} pb {before.PrivateBytes}");
                Console.WriteLine($"After: ws {after.WorkingSetBytes} pb {after.PrivateBytes}");
                Console.WriteLine($"Diff ws {after.WorkingSetBytes - before.WorkingSetBytes} pb {after.PrivateBytes - before.PrivateBytes}");

                (after.WorkingSetBytes - before.WorkingSetBytes).Should().BeGreaterThan((toEat - tolerance).Bytes);
                (after.PrivateBytes - before.PrivateBytes).Should().BeGreaterThan((toEat - tolerance).Bytes);
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
                testProcess.EatPrivateMemory(toEat.Bytes);
                var after = meter.GetMemoryInfo();

                Console.WriteLine($"Before: ws {before.WorkingSetBytes} pb {before.PrivateBytes}");
                Console.WriteLine($"After: ws {after.WorkingSetBytes} pb {after.PrivateBytes}");
                Console.WriteLine($"Diff ws {after.WorkingSetBytes - before.WorkingSetBytes} pb {after.PrivateBytes - before.PrivateBytes}");

                (after.WorkingSetBytes - before.WorkingSetBytes).Should().BeLessThan(tolerance.Bytes);
                (after.PrivateBytes - before.PrivateBytes).Should().BeGreaterThan((toEat - tolerance).Bytes);
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