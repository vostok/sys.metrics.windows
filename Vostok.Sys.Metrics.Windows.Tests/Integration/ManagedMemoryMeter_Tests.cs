using System;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Extensions;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Meters.DotNet;
using Vostok.Sys.Metrics.Windows.PerformanceCounters;
using Vostok.Sys.Metrics.Windows.TestProcess;
using NUnit.Framework;

namespace Vostok.Sys.Metrics.Windows.IntegrationTests
{
    public class ManagedMemoryMeter_Tests
    {
        [TestCase(10, 1)]
        [TestCase(40, 1)]
        [TestCase(10, 2)]
        [TestCase(40, 4)]
        public void AllocationRate(int mb, int seconds)
        {
            var size = DataSize.FromMegabytes(mb);
            var rate = size / seconds;
            
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ManagedMemoryMeter(testProcess.Process.Id))
            {
                meter.GetManagedMemoryInfo();
                testProcess.EatPrivateMemory(size);
                Thread.Sleep(seconds.Seconds());
                testProcess.MakeGC(0);
                var result = meter.GetManagedMemoryInfo();
                (result.Heap.AllocationRate - rate).Bytes.Should().BeLessThan(rate.Bytes / 10);
            }
        }

        [Test]
        public void LohSize()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ManagedMemoryMeter(testProcess.Process.Id))
            {
                var size = DataSize.FromKilobytes(100);
                var lohSizeBefore = meter.GetManagedMemoryInfo().Heap.LargeObjectHeapSize;
                testProcess.EatPrivateMemory(size);
                testProcess.MakeGC(0);
                var lohSizeAfter = meter.GetManagedMemoryInfo().Heap.LargeObjectHeapSize;
                (lohSizeAfter - lohSizeBefore).Bytes.Should().BeGreaterOrEqualTo(size.Bytes);
            }
        }

        [Test]
        public void Gen1Size()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ManagedMemoryMeter(testProcess.Process.Id))
            {
                var size = DataSize.FromKilobytes(10);
                testProcess.MakeGC(2);
                var gen1SizeBefore = meter.GetManagedMemoryInfo().Heap.Gen1Size;
                testProcess.EatPrivateMemory(size);
                testProcess.MakeGC(0);
                var gen1SizeAfter = meter.GetManagedMemoryInfo().Heap.Gen1Size;
                (gen1SizeAfter - gen1SizeBefore).Bytes.Should().BeGreaterOrEqualTo(size.Bytes);
            }
        }
        
        [Test]
        public void Gen2Size()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ManagedMemoryMeter(testProcess.Process.Id))
            {
                var size = DataSize.FromKilobytes(10);
                testProcess.MakeGC(2);
                var gen2SizeBefore = meter.GetManagedMemoryInfo().Heap.Gen2Size;
                testProcess.EatPrivateMemory(size);
                testProcess.MakeGC(0);
                testProcess.MakeGC(1);
                var gen2SizeAfter = meter.GetManagedMemoryInfo().Heap.Gen2Size;
                (gen2SizeAfter - gen2SizeBefore).Bytes.Should().BeGreaterOrEqualTo(size.Bytes);
            }
        }
        
        [Test]
        public void GCMeter_Works()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ManagedMemoryMeter(testProcess.Process.Id))
            {
                var before = meter.GetManagedMemoryInfo().GC;
                Console.WriteLine($"Before: {before}");

                testProcess.MakeGC(0);
                testProcess.MakeGC(1);
                testProcess.MakeGC(2);

                var after = meter.GetManagedMemoryInfo().GC;
                Console.WriteLine($"After: {after}");

                (after.Gen0CollectionsSinceStart - before.Gen0CollectionsSinceStart).Should().BeGreaterOrEqualTo(3); 
                (after.Gen1CollectionsSinceStart - before.Gen1CollectionsSinceStart).Should().BeGreaterOrEqualTo(2);
                (after.Gen2CollectionsSinceStart - before.Gen2CollectionsSinceStart).Should().BeGreaterOrEqualTo(1);
            }
        }

        [Test]
        public void GCMeter_Works_for_current_process()
        {
            using (var meter = new ManagedMemoryMeter())
            {
                var before = meter.GetManagedMemoryInfo().GC;
                Console.WriteLine($"Before: {before}");

                GC.Collect(0);
                GC.Collect(1);
                GC.Collect(2);

                var after = meter.GetManagedMemoryInfo().GC;
                Console.WriteLine($"After: {after}");

                (after.Gen0CollectionsSinceStart - before.Gen0CollectionsSinceStart).Should().BeGreaterOrEqualTo(3); 
                (after.Gen1CollectionsSinceStart - before.Gen1CollectionsSinceStart).Should().BeGreaterOrEqualTo(2);
                (after.Gen2CollectionsSinceStart - before.Gen2CollectionsSinceStart).Should().BeGreaterOrEqualTo(1);
            }
        }

        [Test]
        public void Gen2_increments_gen1_and_gen0()
        {
            using (var testProcess = new TestProcessHandle())
            {
                using (var meter = new ManagedMemoryMeter(testProcess.Process.Id))
                {
                    var before = meter.GetManagedMemoryInfo().GC;
                    Console.WriteLine($"Before: {before}");

                    testProcess.MakeGC(2);

                    var after = meter.GetManagedMemoryInfo().GC;
                    Console.WriteLine($"After: {after}");

                    (after.Gen0CollectionsSinceStart - before.Gen0CollectionsSinceStart).Should().BeGreaterOrEqualTo(1);
                    (after.Gen1CollectionsSinceStart - before.Gen1CollectionsSinceStart).Should().BeGreaterOrEqualTo(1);
                    (after.Gen2CollectionsSinceStart - before.Gen2CollectionsSinceStart).Should().BeGreaterOrEqualTo(1);
                }
            }
        }

        [SetUp]
        public void SetUp()
            => ProcessInstanceNamesCache.Instance.EvictCaches();
    }
}