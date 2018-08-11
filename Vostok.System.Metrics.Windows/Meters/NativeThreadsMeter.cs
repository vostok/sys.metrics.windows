using System;
using System.Collections.Generic;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.Native.Utilities;

namespace Vostok.System.Metrics.Windows.Meters
{
    public class NativeThreadsMeter : IDisposable
    {
        private readonly int pid;

        public NativeThreadsMeter()
            : this(ProcessUtility.CurrentProcessId)
        {
        }

        public NativeThreadsMeter(int pid)
            => this.pid = pid;

        public int GetNativeThreadsCount()
            => NativeThreadsMeterCache.Instance.GetThreadsCount(pid);

        public void Dispose()
        {
        }

        #region Cached threads count for all processes
        private class NativeThreadsMeterCache
        {
            public static readonly NativeThreadsMeterCache Instance = new NativeThreadsMeterCache();

            private readonly object sync;
            private readonly ResizeableBuffer cachedBuffer;
            private readonly TimeCache<Dictionary<int, int>> threadsCountCache;

            private NativeThreadsMeterCache()
            {
                sync = new object();
                cachedBuffer = new ResizeableBuffer();
                threadsCountCache = new TimeCache<Dictionary<int, int>>(BuildCache, () => TimeSpan.FromSeconds(1));
            }

            public int GetThreadsCount(int pid)
            {
                if (TryGetThreadsCount(pid, out var count))
                    return count;

                ProcessUtility.EnsureProcessIsRunning(pid);
                threadsCountCache.Evict();

                if (TryGetThreadsCount(pid, out count))
                    return count;

                throw new InvalidOperationException($"Unable to retrieve process metrics for process with id {pid}");
            }

            private bool TryGetThreadsCount(int pid, out int info)
            {
                var cache = threadsCountCache.GetValue();
                return cache.TryGetValue(pid, out info);
            }

            private unsafe Dictionary<int, int> BuildCache()
            {
                lock (sync)
                {
                    var cache = new Dictionary<int, int>();
                    NtUtility.VisitProcesses(cachedBuffer, info =>
                    {
                        cache[(int)info->UniqueProcessId] = (int)info->NumberOfThreads;
                    });
                    return cache;
                }
            }
        }
        #endregion
    }
}