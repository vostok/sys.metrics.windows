using System;

namespace Vostok.Sys.Metrics.Windows.Helpers
{
    public static class UnsafeHelper
    {
        public static unsafe void CheckBounds(void* start, int length, void* pointer, int structSize)
        {
            if ((ulong) pointer < (ulong) start || (ulong)pointer+(ulong)structSize > (ulong)start+(ulong) length)
                throw new InvalidOperationException("Bounds check failed");
        }
    }
}