using System;

namespace Vostok.Sys.Metrics.Windows.Helpers
{
    internal interface ITimeProvider
    {
        DateTime GetCurrentTime();
        TimeSpan GetIncreasingTime();
    }
}