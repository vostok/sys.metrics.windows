using System;
using FluentAssertions;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Meters.Memory;
using NUnit.Framework;
using System.Threading;
using Vostok.Sys.Metrics.Windows.TestProcess;

namespace Vostok.Sys.Metrics.Windows.IntegrationTests
{
    [TestFixture]
    public class HostMemoryMeter_Tests
    {
        [Test]
        public void Memory_usage_increases_when_process_eats_memory()
        {
            using (var testProcess = new TestProcessHandle())
            using (var memoryMeter = new HostMemoryMeter())
            {
                var toEat = DataSize.FromMegabytes(100);
                var tolerance = DataSize.FromMegabytes(20);
                var beforeEat = memoryMeter.GetHostMemoryInfo().AvailablePhysicalMemory;
                testProcess.EatMemory(toEat);
                Thread.Sleep(100);
                var afterEat = memoryMeter.GetHostMemoryInfo();

                Console.WriteLine($"Before: {beforeEat}. After: {afterEat.AvailablePhysicalMemory}. Diff: {afterEat.AvailablePhysicalMemory - beforeEat}. Total: {afterEat.TotalPhysicalMemory}");

                afterEat.AvailablePhysicalMemory.Should().BeLessThan(beforeEat - toEat + tolerance);
            }
        }
    }
}