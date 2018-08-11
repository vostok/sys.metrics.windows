using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.DotNet
{
    public struct ManagedHeapInfo
    {
        /// <summary>
        /// The maximum data size that can be allocated in generation 0
        /// </summary>
        public long Gen0SizeBytes;
        /// <summary>
        /// The current data size allocated in generation 1
        /// </summary>
        public long Gen1SizeBytes;
        /// <summary>
        /// The current data size allocated in generation 2
        /// </summary>
        public long Gen2SizeBytes;
        /// <summary>
        /// Current size of the Large Object Heap
        /// </summary>
        public long LargeObjectHeapSizeBytes;
        /// <summary>
        /// The current data size allocated on the GC heaps
        /// </summary>
        public long TotalSizeBytes;
        /// <summary>
        /// The rate of allocation on heaps per second
        /// </summary>
        public long AllocatedBytesPerSecond;

        public override string ToString()
        {
            return AllocatedBytesPerSecond == 0
                ? $"Gen1: {new DataSize(Gen1SizeBytes)}, Gen2: {new DataSize(Gen2SizeBytes)}, LOH: {new DataSize(LargeObjectHeapSizeBytes)}"
                : $"Gen1: {new DataSize(Gen1SizeBytes)}, Gen2: {new DataSize(Gen2SizeBytes)}, LOH: {new DataSize(LargeObjectHeapSizeBytes)}, Allocation Rate: {new DataSize(AllocatedBytesPerSecond)}/s";
        }
    }
}