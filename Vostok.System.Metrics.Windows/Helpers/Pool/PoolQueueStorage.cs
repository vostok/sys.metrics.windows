using System.Collections.Concurrent;

namespace Vostok.System.Metrics.Windows.Helpers.Pool
{
    internal class PoolQueueStorage<T> : IPoolStorage<T>
    {
        public PoolQueueStorage()
        {
            queue = new ConcurrentQueue<T>();
        }

        public bool TryAcquire(out T resource)
        {
            return queue.TryDequeue(out resource);
        }

        public void Put(T resource)
        {
            queue.Enqueue(resource);
        }

        public int Count => queue.Count;

        private readonly ConcurrentQueue<T> queue;
    }
}