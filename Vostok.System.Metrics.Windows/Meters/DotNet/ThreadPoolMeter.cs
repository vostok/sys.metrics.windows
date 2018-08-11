using System;
using System.Threading;

namespace Vostok.System.Metrics.Windows.Meters.DotNet
{
    public class ThreadPoolMeter : IDisposable
    {
        /// <summary>
        /// Returns current state of .NET threadpool
        /// </summary>
        public ThreadPoolState GetThreadPoolState()
        {
            ThreadPool.GetMinThreads(out var workerThreadsMin, out var completionPortThreadsMin);
            ThreadPool.GetMaxThreads(out var workerThreadsMax, out var completionPortThreadsMax);
            ThreadPool.GetAvailableThreads(out var workerThreadsAvailable, out var completionPortThreadsAvailable);
            return new ThreadPoolState(workerThreadsMin, workerThreadsMax - workerThreadsAvailable, completionPortThreadsMin, completionPortThreadsMax - completionPortThreadsAvailable);
        }

        public void Dispose()
        {
        }
    }
}