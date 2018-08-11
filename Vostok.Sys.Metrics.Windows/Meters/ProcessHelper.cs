using System;
using System.Collections.Generic;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Native.Structures;
using Vostok.Sys.Metrics.Windows.Native.Utilities;

namespace Vostok.Sys.Metrics.Windows.Meters
{
    public class ProcessHelper
    {
        public static readonly ProcessHelper Instance = new ProcessHelper();

        private readonly ResizeableBuffer buffer;
        private readonly object sync;

        public ProcessHelper()
        {
            buffer = new ResizeableBuffer();
            sync = new object();
        }

        public bool Is64Bit(int pid)
        {
            return ProcessUtility.Is64BitProcess(pid);
        }

        public unsafe List<ProcessInfo> GetAllProcesses()
        {
            lock (sync)
            {
                var result = new List<ProcessInfo>();
                NtUtility.VisitProcesses(buffer, info =>
                {
                    Console.WriteLine(new string((char*) info->NamePtr) + ": " + info->NumberOfThreads);
                    FillResult(result, info);
                });
                return result;
            }
        }

        private unsafe void FillResult(List<ProcessInfo> result, SystemProcessInformation* proc)
        {
            var info = new ProcessInfo
            {
                Id = (int) proc->UniqueProcessId,
                Name = new string((char*) proc->NamePtr)
            };
            result.Add(info);
        }
    }
}