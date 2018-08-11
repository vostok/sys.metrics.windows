using System;
using Vostok.System.Metrics.Windows.Helpers;
using Vostok.System.Metrics.Windows.Native.Constants;
using Vostok.System.Metrics.Windows.Native.Libraries;
using Vostok.System.Metrics.Windows.Native.Structures;

namespace Vostok.System.Metrics.Windows.Native.Utilities
{
    internal static class NtUtility
    {
        public static unsafe void VisitProcesses(ResizeableBuffer resizeableBuffer, SystemProcInfoVisitor visit)
        {
            var size = 100 * 1024;

            while (true)
            {
                var buffer = resizeableBuffer.Get(ref size, false);
                fixed (byte* ptr = buffer)
                {
                    var status = NtDll.NtQuerySystemInformation(
                        SYSTEM_INFORMATION_CLASS.SystemProcessInformation,
                        (IntPtr) ptr,
                        buffer.Length,
                        out size);
                    if (status == NtStatus.STATUS_INFO_LENGTH_MISMATCH)
                        continue;

                    if (!status.IsSuccessful())
                        throw new InvalidOperationException($"Can't query SystemProcessInformation. NtStatus: 0x{status:X}");

                    VisitProcesses(ptr, buffer, visit);
                    return;
                }
            }
        }

        private static unsafe void VisitProcesses(byte* ptr, byte[] buffer, SystemProcInfoVisitor visit)
        {
            while (true)
            {
                UnsafeHelper.CheckBounds(ptr, buffer.Length, ptr, sizeof(SystemProcessInformation));
                var processInfo = (SystemProcessInformation*) ptr;
                
                // don't visit processes which maybe suspended zombies
                // (which may appear in NtQuerySystemInformation output)
                // to avoid survival of zombie processes due to opened handles
                // TODO rewrite this comment
                if (!IsZombie(processInfo))
                    visit(processInfo);

                if (processInfo->NextEntryOffset == 0)
                    return;
                
                ptr += processInfo->NextEntryOffset;
            }
        }

        private static unsafe bool IsZombie(SystemProcessInformation* info) => info->NumberOfThreads == 0;

        internal unsafe delegate void SystemProcInfoVisitor(SystemProcessInformation* info);
    }
}