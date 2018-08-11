﻿using System;
using FluentAssertions.Extensions;
using Vostok.System.Metrics.Windows.Meters.Memory;
using Vostok.System.Metrics.Windows.TestsCore;
using NUnit.Framework;

namespace Vostok.System.Metrics.Windows.IntegrationTests
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
                    () => fileCacheSizeMeter.GetCacheSize(),
                    100.Milliseconds(), 5);
                Console.WriteLine(string.Join(", ", results));
            }
        }
    }
}