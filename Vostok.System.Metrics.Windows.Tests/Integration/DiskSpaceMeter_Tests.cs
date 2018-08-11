using System;
using FluentAssertions.Extensions;
using Vostok.System.Metrics.Windows.Meters.Disk;
using Vostok.System.Metrics.Windows.TestsCore;
using NUnit.Framework;

namespace Vostok.System.Metrics.Windows.IntegrationTests
{
    [TestFixture]
    public class DiskSpaceMeter_Tests
    {
        [Test]
        public void GetFreeSpace_returns_some_values_and_does_not_throw_exceptions()
        {
            var letter = DriveTestHelpers.GetDriveLetter();
            Console.WriteLine($"Drive letter: {letter}");

            using (var meter = new DiskSpaceMeter(letter))
            {
                var freeSpaceResults = TestHelpers.GetMeterValues(
                    () => meter.GetFreeSpace(),
                    100.Milliseconds(), 5);
                Console.WriteLine(string.Join(", ", freeSpaceResults));
            }
        }

        [Test]
        public void GetTotalSpace_returns_some_values_and_does_not_throw_exceptions()
        {
            var letter = DriveTestHelpers.GetDriveLetter();
            Console.WriteLine($"Drive letter: {letter}");

            using (var meter = new DiskSpaceMeter(letter))
            {
                var totalSpaceResults = TestHelpers.GetMeterValues(
                    () => meter.GetTotalSpace(),
                    100.Milliseconds(), 5);
                Console.WriteLine(string.Join(", ", totalSpaceResults));
            }
        }
    }
}