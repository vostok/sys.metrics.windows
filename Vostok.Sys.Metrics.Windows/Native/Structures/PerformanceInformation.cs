using System;

namespace Vostok.Sys.Metrics.Windows.Native.Structures
{
    internal struct PERFORMANCE_INFORMATION
    {
        public int cb;
        public IntPtr CommitTotal;
        public IntPtr CommitLimit;
        public IntPtr CommitPeak;
        public IntPtr PhysicalTotal;
        public IntPtr PhysicalAvailable;
        public IntPtr SystemCache;
        public IntPtr KernelTotal;
        public IntPtr KernelPaged;
        public IntPtr KernelNonpaged;
        public IntPtr PageSize;
        public int HandleCount;
        public int ProcessCount;
        public int ThreadCount;
    }
}