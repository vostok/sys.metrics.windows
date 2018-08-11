using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.DotNet
{
    public struct ManagedHeapInfo
    {
        /// <summary>
        /// The maximum data size that can be allocated in generation 0
        /// </summary>
        public DataSize Gen0Size;
        /// <summary>
        /// The current data size allocated in generation 1
        /// </summary>
        public DataSize Gen1Size;
        /// <summary>
        /// The current data size allocated in generation 2
        /// </summary>
        public DataSize Gen2Size;
        /// <summary>
        /// Current size of the Large Object Heap
        /// </summary>
        public DataSize LargeObjectHeapSize;
        /// <summary>
        /// The curent data size allocated on the GC heaps
        /// </summary>
        public DataSize TotalSize;
        /// <summary>
        /// The rate of allocation on heaps per second
        /// </summary>
        public DataSize AllocationRate;

        public override string ToString()
        {
            return AllocationRate.Bytes == 0
                ? $"Gen1: {Gen1Size}, Gen2: {Gen2Size}, LOH: {LargeObjectHeapSize}"
                : $"Gen1: {Gen1Size}, Gen2: {Gen2Size}, LOH: {LargeObjectHeapSize}, Allocation Rate: {AllocationRate}/s";
        }
    }
}