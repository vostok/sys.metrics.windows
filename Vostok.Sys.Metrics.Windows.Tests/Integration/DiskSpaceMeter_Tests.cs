using System;
using FluentAssertions.Extensions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Meters.Disk;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
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
                    () => meter.GetFreeSpaceBytes(),
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
                    () => meter.GetTotalSpaceBytes(),
                    100.Milliseconds(), 5);
                Console.WriteLine(string.Join(", ", totalSpaceResults));
            }
        }
    }
}