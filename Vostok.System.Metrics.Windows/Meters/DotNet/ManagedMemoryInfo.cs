namespace Vostok.System.Metrics.Windows.Meters.DotNet
{
    public class ManagedMemoryInfo
    {
        public GarbageCollectionInfo GC;
        public ManagedHeapInfo Heap;

        public override string ToString()
            => $"GC: {GC}; Heap: {Heap}";
    }
}