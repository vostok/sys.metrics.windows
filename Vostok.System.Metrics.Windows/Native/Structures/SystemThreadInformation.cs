using System;
using System.Runtime.InteropServices;

namespace Vostok.System.Metrics.Windows.Native.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemThreadInformation
    {
        public long KernelTime;
        public long UserTime;
        public long CreateTime;
        public uint WaitTime;
        public IntPtr StartAddress;
        public IntPtr UniqueProcess;
        public IntPtr UniqueThread;
        public int Priority;
        public int BasePriority;
        public uint ContextSwitches;
        public uint ThreadState;
        public uint WaitReason;
    }
}