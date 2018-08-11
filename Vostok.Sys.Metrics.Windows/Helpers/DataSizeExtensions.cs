using System;

namespace Vostok.Sys.Metrics.Windows.Helpers
{
    internal static class DataSizeExtensions
    {
        private const long PBit = 1000L * 1000 * 1000 * 1000 * 1000;
        private const long TBit = 1000L * 1000 * 1000 * 1000;
        private const long GBit = 1000L * 1000 * 1000;
        private const long MBit = 1000L * 1000;
        private const long KBit = 1000L;
        
        public static string ToStringAsDecimalBits(this DataSize dataSize, bool shortFormat=true)
        {
            var bits = Math.Abs(dataSize.Bytes * 8);
            if (bits >= PBit)
                return (bits / PBit).ToString("0.##") + ' ' + (shortFormat ? "PBit" : "petabits");

            if (bits >= TBit)
                return (bits / TBit).ToString("0.##") + ' ' + (shortFormat ? "TBit" : "terabits");

            if (bits >= GBit)
                return (bits / GBit).ToString("0.##") + ' ' + (shortFormat ? "GBit" : "gigabits");

            if (bits >= MBit)
                return (bits / MBit).ToString("0.##") + ' ' + (shortFormat ? "MBit" : "megabits");

            if (bits >= KBit)
                return (bits / KBit).ToString("0.##") + ' ' + (shortFormat ? "KBit" : "kilobits");

            return bits.ToString() + ' ' + "bit";
        }
    }
}