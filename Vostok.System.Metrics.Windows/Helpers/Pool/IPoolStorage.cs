namespace Vostok.System.Metrics.Windows.Helpers.Pool
{
    internal interface IPoolStorage<T>
    {
        bool TryAcquire(out T resource);

        void Put(T resource);

        int Count { get; }
    }
}