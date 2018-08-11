using System;
using System.Threading;

namespace Vostok.System.Metrics.Windows.Helpers
{
    internal class TimeCache
    {
        private readonly Func<double> measure;
        private readonly Func<TimeSpan> getMinPauseBetweenObservations;
        private readonly ITimeProvider timeProvider;

        private int sync;
        private DateTime lastMeasureTime;
        private double cachedValue;

        public TimeCache(
            Func<double> measure,
            Func<TimeSpan> getMinPauseBetweenObservations,
            ITimeProvider timeProvider = null)
        {
            this.measure = measure;
            this.getMinPauseBetweenObservations = getMinPauseBetweenObservations;
            this.timeProvider = timeProvider ?? TimeProvider.Instance;
            lastMeasureTime = DateTime.MinValue;
            sync = 0;
        }

        public double GetValue()
        {
            try
            {
                if (Interlocked.Increment(ref sync) == 1)
                {
                    var now = timeProvider.GetCurrentTime();
                    if (lastMeasureTime + getMinPauseBetweenObservations() <= now)
                    {
                        var result = measure();
                        Interlocked.Exchange(ref cachedValue, result);
                        lastMeasureTime = now;
                    }
                }

                return cachedValue;
            }
            finally
            {
                Interlocked.Decrement(ref sync);
            }
        }
    }

    internal class TimeCache<T> where T : class
    {
        private readonly Func<T> measure;
        private readonly Func<TimeSpan> getMinPauseBetweenObservations;
        private readonly ITimeProvider timeProvider;
        private readonly object sync = new object();
        
        private TimeSpan lastMeasureTime;
        private volatile T cachedValue;

        public TimeCache(
            Func<T> measure,
            Func<TimeSpan> getMinPauseBetweenObservations,
            ITimeProvider timeProvider = null)
        {
            this.measure = measure;
            this.getMinPauseBetweenObservations = getMinPauseBetweenObservations;
            this.timeProvider = timeProvider ?? TimeProvider.Instance;
            lastMeasureTime = TimeSpan.MinValue;
        }

        public T GetValue()
        {
            if (!NeedRefresh())
                return cachedValue;
            lock (sync)
            {
                if (!NeedRefresh())
                    return cachedValue;
                
                var result = measure();
                //CR(ezsilmar) На кой здесь интерлокед? И так все под локом.
                Interlocked.Exchange(ref cachedValue, result);
                lastMeasureTime = timeProvider.GetIncreasingTime();
                return result;
            }
        }

        public void Evict()
        {
            lock (sync)
                lastMeasureTime = TimeSpan.MinValue;
        }

        private bool NeedRefresh()
        {
            return lastMeasureTime + getMinPauseBetweenObservations() <= timeProvider.GetIncreasingTime();
        }
    }
}