using System;
using System.Threading;

namespace Vostok.Sys.Metrics.Windows.Helpers.Pool
{
    // pool from core-infra module
    internal class Pool<T> : IPool<T>
        where T : class
    {
        public Pool(Func<T> resourceFactory, PoolAccessStrategy accessStrategy = PoolAccessStrategy.FIFO)
        {
            this.resourceFactory = resourceFactory;

            resourceStorage = CreateStorage(accessStrategy);
        }

        public T Acquire()
        {
            CheckNotDisposed();

            if (!resourceStorage.TryAcquire(out var resource))
            {
                resource = resourceFactory();
                Interlocked.Increment(ref allocated);
            }

            return resource;
        }

        public void Release(T resource)
        {
            if (disposed)
            {
                TryDispose(resource);
            }
            else
            {
                resourceStorage.Put(resource);
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                while (resourceStorage.TryAcquire(out var resource))
                    TryDispose(resource);
            }
        }

        public int Allocated => allocated;

        public int Available => resourceStorage.Count;

        private void CheckNotDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("pool", "Can't operate on disposed pool.");
        }

        private static void TryDispose(T resource)
        {
            var disposable = resource as IDisposable;
            disposable?.Dispose();
        }

        private static IPoolStorage<T> CreateStorage(PoolAccessStrategy strategy)
        {
            switch (strategy)
            {
                case PoolAccessStrategy.FIFO:
                    return new PoolQueueStorage<T>();

                case PoolAccessStrategy.LIFO:
                    return new PoolStackStorage<T>();

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), "Unexpected access strategy: " + strategy);
            }
        }

        private readonly Func<T> resourceFactory;
        private readonly IPoolStorage<T> resourceStorage;

        private int allocated;
        private volatile bool disposed;
    }
}