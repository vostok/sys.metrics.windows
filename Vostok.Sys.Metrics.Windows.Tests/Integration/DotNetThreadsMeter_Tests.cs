using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Meters.DotNet;
using Vostok.Sys.Metrics.Windows.TestProcess;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
{
    [TestFixture]
    public class DotNetThreadsMeter_Tests
    {
        [Test]
        public void Measures_physical_threads()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new DotNetThreadsMeter(testProcess.Process.Id))
            {
                var before = meter.GetDotNetPhysicalThreadsCount();
                Console.WriteLine($"Before: {before}");

                testProcess.MakeThreads(10);

                TestHelpers.ShouldPassIn(() =>
                {
                    var after = meter.GetDotNetPhysicalThreadsCount();
                    Console.WriteLine($"After: {after}");
                    after.Should().BeGreaterOrEqualTo(before + 10);
                }, 2.Seconds(), 100.Milliseconds());
            }
        }
    }
}