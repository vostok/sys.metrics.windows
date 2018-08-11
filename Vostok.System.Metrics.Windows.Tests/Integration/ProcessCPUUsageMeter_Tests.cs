using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using Vostok.System.Metrics.Windows.Meters.CPU;
using Vostok.System.Metrics.Windows.TestProcess;
using Vostok.System.Metrics.Windows.TestsCore;
using NUnit.Framework;

namespace Vostok.System.Metrics.Windows.IntegrationTests
{
    [TestFixture]
    public class ProcessCPUUsageMeter_Tests
    {
        [Test]
        public void Measures_CPU_usage_for_process()  //TODO (epeshk): should we set limits or use processor affinity to make this test more stable?
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new ProcessCpuUsageMeter(testProcess.Process.Id))
            {
                var observationsCount = 20;
                var lowValues = TestHelpers.GetMeterValues(() => meter.GetCpuUsage(), TimeSpan.FromMilliseconds(100), observationsCount);
                testProcess.EatCpu(1);
                var highValues = TestHelpers.GetMeterValues(() => meter.GetCpuUsage(), TimeSpan.FromMilliseconds(100), observationsCount);

                Console.WriteLine($"Low values: {string.Join(", ", lowValues)}");
                Console.WriteLine($"High values: {string.Join(", ", highValues)}");

                highValues
                    .Count(h => lowValues.All(l => l < h))
                    .Should()
                    .BeGreaterOrEqualTo((int)(highValues.Length * 0.75));
            }
        }

        [Test]
        public void First_call_to_meter_returns_zero()
        {
            using (var meter = new ProcessCpuUsageMeter())
            {
                var usage = meter.GetCpuUsage();
                usage.Should().Be(0);
            }
        }

        [Test]
        public void Value_is_cached_for_next_250_milliseconds()
        {
            var cachePeriod = 250.Milliseconds();
            using (var meter = new ProcessCpuUsageMeter())
            {
                // warmup meter
                TestHelpers.GetMeterValues(() => meter.GetCpuUsage(), cachePeriod, 2);
                var results = TestHelpers.GetMeterValues(
                    () => meter.GetCpuUsage(),
                    ((cachePeriod - 20.Milliseconds()).Ticks/5).Ticks(),
                    5);
                results.Should().AllBeEquivalentTo(results[0]);
            }
        }
    }
}