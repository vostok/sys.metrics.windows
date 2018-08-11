using System;
using System.Runtime.InteropServices;
using Vostok.Sys.Metrics.Windows.Native.Constants;

// ReSharper disable InconsistentNaming

namespace Vostok.Sys.Metrics.Windows.Native.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct GROUP_AFFINITY {
        public IntPtr Mask;
        public ushort Group;
        public fixed ushort Reserved[3];
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct PROCESSOR_RELATIONSHIP {
        public byte Flags;
        public byte EfficiencyClass;
        public fixed byte Reserved[20];
        // https://docs.microsoft.com/en-us/windows/desktop/api/winnt/ns-winnt-_processor_relationship
        // If the PROCESSOR_RELATIONSHIP structure represents a processor core, the GroupCount member is always 1.
        public ushort GroupCount;
        public GROUP_AFFINITY GroupMask; // [ANYSIZE_ARRAY]
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX {
        public LOGICAL_PROCESSOR_RELATIONSHIP Relationship;
        public int Size;
        public PROCESSOR_RELATIONSHIP Processor;
    }
}