using System;

namespace Vostok.System.Metrics.Windows.Helpers
{
    internal class ResizeableBuffer
    {
        private readonly int minSizeForAllocation;
        private readonly int minSizeForDecrease;
        
        private byte[] array = Array.Empty<byte>();
        public byte[] Buffer => array;
        
        public ResizeableBuffer(int minSizeForAllocation = 16, int minSizeForDecrease = 16*1024)
        {
            this.minSizeForAllocation = minSizeForAllocation;
            this.minSizeForDecrease = minSizeForDecrease;
        }

        public byte[] Get(int size, bool reallocateLargeBuffers = true)
        {
            if (array.Length >= size)
            {
                // if existing buffer is too large - create new
                if (reallocateLargeBuffers && IsTooLarge(size))
                    Reallocate(size);
                return array;
            }
            Reallocate(size);
            return array;
        }

        public byte[] Get(ref int size, bool reallocateLargeBuffers=true)
        {
            var arr = Get(size, reallocateLargeBuffers);
            size = arr.Length;
            return arr;
        }

        private void Reallocate(int minSize) => array = new byte[GetSize(minSize)];

        private int GetSize(int minSize) =>
            minSize > int.MaxValue / 4 ? minSize : Math.Max(4 * minSize / 3, minSizeForAllocation);
        
        private bool IsTooLarge(int size) => array.Length >= 2 * size && array.Length >= minSizeForDecrease;
    }
}