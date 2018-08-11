using System;

namespace Vostok.Sys.Metrics.Windows.Helpers.Pool
{
    internal interface IPool<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Acquires a resource from pool, allocating a new one if necessary.
        /// </summary>
        T Acquire();

        /// <summary>
        /// Releases a previously acquired resource back to pool.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Attempt to release a resource that wasn't acquired earlier.</exception>
        void Release(T resource);

        /// <summary>
        /// Returns the total count of resources ever allocated by this pool.
        /// </summary>
        int Allocated { get; }

        /// <summary>
        /// Returns the count of resources currently free for use in this pool.
        /// </summary>
        int Available { get; }
    }
}