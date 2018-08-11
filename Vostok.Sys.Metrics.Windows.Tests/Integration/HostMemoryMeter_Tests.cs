using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Meters.Memory;
using Vostok.Sys.Metrics.Windows.TestProcess;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
{
    [TestFixture]
    public class HostMemoryMeter_Tests
    {
        [Test, Explicit]
        public void Memory_usage_increases_when_process_eats_memory()
        {
            using (var testProcess = new TestProcessHandle())
            using (var memoryMeter = new HostMemoryMeter())
            {
                var toEat = DataSize.FromMegabytes(100).Bytes;
                var tolerance = DataSize.FromMegabytes(20).Bytes;
                var beforeEat = memoryMeter.GetHostMemoryInfo().AvailablePhysicalMemoryBytes;
                testProcess.EatMemory(toEat);
                Thread.Sleep(100);
                var afterEat = memoryMeter.GetHostMemoryInfo();

                Console.WriteLine($"Before: {beforeEat}. After: {afterEat.AvailablePhysicalMemoryBytes}. Diff: {afterEat.AvailablePhysicalMemoryBytes - beforeEat}. Total: {afterEat.TotalPhysicalMemoryBytes}");

                afterEat.AvailablePhysicalMemoryBytes.Should().BeLessThan(beforeEat - toEat + tolerance);
            }
        }
    }
}