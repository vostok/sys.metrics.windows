// ReSharper disable InconsistentNaming
namespace Vostok.Sys.Metrics.Windows.Native.Constants
{
    internal enum NtStatus : uint
    {
        STATUS_INFO_LENGTH_MISMATCH = 0xC0000004
    }

    internal static class NtStatusExtensions
    {
        public static bool IsSuccessful(this NtStatus status) => (uint) status <= int.MaxValue;
    }

}