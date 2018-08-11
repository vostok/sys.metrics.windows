using System;

namespace Vostok.System.Metrics.Windows.Helpers
{
    internal interface ITimeProvider
    {
        DateTime GetCurrentTime();
        TimeSpan GetIncreasingTime();
    }
}