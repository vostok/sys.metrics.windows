using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using NSubstitute;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Tests.Unit
{

    [TestFixture]
    public class TimeCache_Tests
    {
        private readonly DateTime first = new DateTime(1, 1, 1, 1, 1, 1, 0);

        [Test]
        public void Should_not_call_measure_if_period_is_not_elapsed()
        {
            var callsCount = 0;
            var (cache, _) = GetTimeCache(() => callsCount++, 10.Milliseconds());

            cache.GetValue();
            cache.GetValue();

            callsCount.Should().Be(1);
        }

        [Test]
        public void Should_call_measure_after_period_elapsed()
        {
            var callsCount = 0;
            var (cache, timeProvider) = GetTimeCache(() => callsCount++, 10.Milliseconds());

            cache.GetValue();
            timeProvider.GetCurrentTime().Returns(first + 10.Milliseconds());
            cache.GetValue();

            callsCount.Should().Be(2);
        }

        [Test]
        public void If_measure_failed_with_exception_should_retry_at_next_call()
        {
            var (cache, _) = GetTimeCache(() => throw new Exception(), 10.Milliseconds());

            Assert.Throws<Exception>(() => cache.GetValue());
            Assert.Throws<Exception>(() => cache.GetValue());
        }

        private (TimeCache, ITimeProvider) GetTimeCache(Func<double> measure, TimeSpan period)
        {
            var timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.GetCurrentTime().Returns(first);
            return (new TimeCache(measure, () => period, timeProvider), timeProvider);
        }
    }
}