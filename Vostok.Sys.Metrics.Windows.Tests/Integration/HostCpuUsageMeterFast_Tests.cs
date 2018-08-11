using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Meters.CPU;
using Vostok.Sys.Metrics.Windows.TestProcess;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
{
    [TestFixture]
    public class HostCpuUsageMeterFast_Tests
    {
        [Test]
        public void Measures_total_CPU_usage()
        {
            using (var testProcess = new TestProcessHandle())
            {
                var meter = new HostCpuUsageMeterFast();
                var cores = ProcessorInfo.LogicalCores;

                var observationsCount = 10;
                var lowValues = TestHelpers.GetMeterValues(
                    () => meter.GetHostCpuUsage()*cores,
                    TimeSpan.FromMilliseconds(255),
                    observationsCount);

                testProcess.EatCpu(cores - 1);

                var highValues = TestHelpers.GetMeterValues(
                    () => meter.GetHostCpuUsage() * cores,
                    TimeSpan.FromMilliseconds(255),
                    observationsCount);

                Console.WriteLine($"Low values: {string.Join(", ", lowValues)}");
                Console.WriteLine($"High values: {string.Join(", ", highValues)}");
                
                highValues
                    .Count(h => lowValues.Count(l => l < h) > observationsCount*0.75)
                    .Should()
                    .BeGreaterOrEqualTo((int)(observationsCount * 0.75));
            }
        }

        [Test]
        public void First_call_to_meter_returns_zero()
        {
            var meter = new HostCpuUsageMeterFast();
            var usage = meter.GetHostCpuUsage();
            usage.Should().Be(0);
        }

        [Test]
        public void Value_is_cached_for_next_250_milliseconds()
        {
            var cachePeriod = 250.Milliseconds();
            var meter = new HostCpuUsageMeterFast();
            // warmup meter
            TestHelpers.GetMeterValues(() => meter.GetHostCpuUsage(), cachePeriod, 2);
            var results = TestHelpers.GetMeterValues(
                () => meter.GetHostCpuUsage(),
                ((cachePeriod - 20.Milliseconds()).Ticks / 5).Ticks(),
                5);
            results.Should().AllBeEquivalentTo(results[0]);
        }
    }
}