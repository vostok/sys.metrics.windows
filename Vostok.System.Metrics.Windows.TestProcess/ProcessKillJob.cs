using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Vostok.System.Metrics.Windows.TestProcess
{
    internal class ProcessKillJob
    {
        private readonly IntPtr jobHandle;
        private readonly IntPtr jobObjectInfoPtr;

        public ProcessKillJob()
        {
            jobHandle = CreateJobObject(IntPtr.Zero, IntPtr.Zero);
            if (jobHandle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            try
            {
                var jobObjectInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
                {
                    BasicLimitInformation = new JOBOBJECT_BASIC_LIMIT_INFORMATION
                    {
                        LimitFlags = JOBOBJECT_BASIC_LIMIT_FLAGS.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
                    }
                };

                var jobObjectInfoLength = Marshal.SizeOf(jobObjectInfo);
                jobObjectInfoPtr = Marshal.AllocHGlobal(jobObjectInfoLength);

                try
                {
                    Marshal.StructureToPtr(jobObjectInfo, jobObjectInfoPtr, false);

                    if (!SetInformationJobObject(jobHandle,
                        JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,
                        jobObjectInfoPtr,
                        (uint) jobObjectInfoLength))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                catch (Exception)
                {
                    Marshal.FreeHGlobal(jobObjectInfoPtr);
                    throw;
                }
            }
            catch (Exception)
            {
                CloseHandle(jobHandle);
                throw;
            }
        }

        public void Dispose()
        {
            CloseHandle(jobHandle);
            Marshal.FreeHGlobal(jobObjectInfoPtr);
        }

        public void AddProcess(Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }
            AddProcess(process.Handle);
        }

        public void AddProcess(IntPtr processHandle)
        {
            if (!AssignProcessToJobObject(jobHandle, processHandle))
                throw new Win32Exception();
        }
        
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, IntPtr lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetInformationJobObject(IntPtr hJob, JOBOBJECTINFOCLASS JobObjectInfoClass, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

    }
    
    internal enum JOBOBJECTINFOCLASS
    {
        JobObjectBasicLimitInformation = 2,
        JobObjectBasicUIRestrictions = 4,
        JobObjectSecurityLimitInformation = 5,
        JobObjectEndOfJobTimeInformation = 6,
        JobObjectAssociateCompletionPortInformation = 7,
        JobObjectExtendedLimitInformation = 9,
        JobObjectGroupInformation = 11, // 0x0000000B
        JobObjectNotificationLimitInformation = 12, // 0x0000000C
        JobObjectGroupInformationEx = 14, // 0x0000000E
        JobObjectCpuRateControlInformation = 15, // 0x0000000F
        JobObjectNetRateControlInformation = 32, // 0x00000020
        JobObjectNotificationLimitInformation2 = 34, // 0x00000022
        JobObjectLimitViolationInformation2 = 35, // 0x00000023
    }
    
    internal struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    {
        public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
        public IO_COUNTERS IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }
    internal struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    {
        public long PerProcessUserTimeLimit;
        public long PerJobUserTimeLimit;
        public JOBOBJECT_BASIC_LIMIT_FLAGS LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public uint ActiveProcessLimit;
        public UIntPtr Affinity;
        public uint PriorityClass;
        public uint SchedulingClass;
    }
    
    internal struct IO_COUNTERS
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }
    
    internal enum JOBOBJECT_BASIC_LIMIT_FLAGS : uint
    {
        JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 8,
        JOB_OBJECT_LIMIT_AFFINITY = 16, // 0x00000010
        JOB_OBJECT_LIMIT_BREAKAWAY_OK = 2048, // 0x00000800
        JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 1024, // 0x00000400
        JOB_OBJECT_LIMIT_JOB_MEMORY = 512, // 0x00000200
        JOB_OBJECT_LIMIT_JOB_TIME = 4,
        JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 8192, // 0x00002000
        JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 64, // 0x00000040
        JOB_OBJECT_LIMIT_PRIORITY_CLASS = 32, // 0x00000020
        JOB_OBJECT_LIMIT_PROCESS_MEMORY = 256, // 0x00000100
        JOB_OBJECT_LIMIT_PROCESS_TIME = 2,
        JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 128, // 0x00000080
        JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 4096, // 0x00001000
        JOB_OBJECT_LIMIT_SUBSET_AFFINITY = 16384, // 0x00004000
        JOB_OBJECT_LIMIT_WORKINGSET = 1,
    }
}