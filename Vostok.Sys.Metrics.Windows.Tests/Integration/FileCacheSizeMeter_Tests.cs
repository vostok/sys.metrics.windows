using System;
using FluentAssertions.Extensions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Meters.Memory;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
{
    [TestFixture]
    public class FileCacheSizeMeter_Tests
    {
        [Test]
        public void Return_some_values_and_does_not_throw_exceptions()
        {
            using (var fileCacheSizeMeter = new FileCacheSizeMeter())
            {
                var results = TestHelpers.GetMeterValues(
                    () => fileCacheSizeMeter.GetFileCacheSizeBytes(),
                    100.Milliseconds(), 5);
                Console.WriteLine(string.Join(", ", results));
            }
        }
    }
}