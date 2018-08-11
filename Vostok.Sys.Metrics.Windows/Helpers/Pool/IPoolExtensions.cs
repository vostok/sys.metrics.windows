using System.Collections.Generic;

namespace Vostok.Sys.Metrics.Windows.Helpers.Pool
{
    internal static class IPoolExtensions
    {
        /// <summary>
        /// Acquires a resource from pool and wraps it into a disposable handle which releases resource on disposal.
        /// </summary>
        public static PoolHandle<T> AcquireHandle<T>(this IPool<T> pool)
            where T : class
        {
            return new PoolHandle<T>(pool, pool.Acquire());
        }

        public static void Preallocate<T>(this IPool<T> pool, int count)
            where T : class
        {
            var resources = new List<T>();

            for (int i = 0; i < count; i++)
            {
                resources.Add(pool.Acquire());
            }

            foreach (var resource in resources)
            {
                pool.Release(resource);
            }
        }
    }
}