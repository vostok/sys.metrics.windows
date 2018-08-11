using System;
using System.Linq;
using FluentAssertions.Extensions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Meters.Disk;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
{
    [TestFixture]
    public class LogicalDiskMeter_Tests
    {
        [Test]
        public void Returns_some_values_and_does_not_throw_exceptions()
        {
            var letter = DriveTestHelpers.GetDriveLetter();
            Console.WriteLine($"Drive letter: {letter}");

            using (var meter = new LogicalDiskMeter(letter[0]))
            {
                var results = TestHelpers.GetMeterValues(
                    () => meter.GetDiskMetrics(),
                    1.Seconds(), 5);
                Console.WriteLine(string.Join(Environment.NewLine, results.Select(x => x.ToString())));
            }
        }
        
        [Test]
        public void AllDisksMeter_Returns_some_values_and_does_not_throw_exceptions()
        {
            using (var meter = new AllLogicalDisksMeter())
            {
                var results = TestHelpers.GetMeterValues(
                    () => meter.GetDiskMetrics(),
                    1.Seconds(), 5);
                Console.WriteLine(string.Join(Environment.NewLine, results.Select(x => x.ToString())));
            }
        }
    }
}