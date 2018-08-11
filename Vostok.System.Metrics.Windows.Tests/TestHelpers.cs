using System;
using System.Diagnostics;
using System.Threading;

namespace Vostok.System.Metrics.Windows.TestsCore
{
    public static class TestHelpers
    {
        public static T[] GetMeterValues<T>(Func<T> gauge, TimeSpan observationPeriod, int observationsCount)
        {
            var result = new T[observationsCount];
            for (var i = 0; i < observationsCount; i++)
            {
                result[i] = gauge();
                Thread.Sleep(observationPeriod);
            }
            return result;
        }

        public static void ShouldPassIn(this Action action, TimeSpan timeout, TimeSpan checkPeriod)
        {
            var sw = Stopwatch.StartNew();
            while (sw.Elapsed < timeout)
            {
                try
                {
                    action();
                    return;
                }
                catch { }
                Thread.Sleep(checkPeriod);
            }

            action();
        }
    }
}
