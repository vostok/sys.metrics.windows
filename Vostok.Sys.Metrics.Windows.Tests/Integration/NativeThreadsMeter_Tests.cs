using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using Vostok.Sys.Metrics.Windows.Meters;
using Vostok.Sys.Metrics.Windows.TestProcess;
using Vostok.Sys.Metrics.Windows.TestsCore;
using NUnit.Framework;

namespace Vostok.Sys.Metrics.Windows.IntegrationTests
{
    [TestFixture]
    public class NativeThreadsMeter_Tests
    {
        [Test]
        public void Measures_threads()
        {
            using (var testProcess = new TestProcessHandle())
            using (var meter = new NativeThreadsMeter(testProcess.Process.Id))
            {
                var before = meter.GetNativeThreadsCount();
                Console.WriteLine($"Before: {before}");

                testProcess.MakeThreads(10);

                TestHelpers.ShouldPassIn(() =>
                {
                    var after = meter.GetNativeThreadsCount();
                    Console.WriteLine($"After: {after}");
                    after.Should().BeGreaterOrEqualTo(before + 10);
                }, 2.Seconds(), 100.Milliseconds());
            }
        }
    }
}