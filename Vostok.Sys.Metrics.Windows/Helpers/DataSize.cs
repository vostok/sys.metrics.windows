using System;

namespace Vostok.Sys.Metrics.Windows.Helpers
{
    internal struct DataSize : IEquatable<DataSize>, IComparable<DataSize>
    {
        private const long Kilobyte = 1024;

        private const long Megabyte = Kilobyte * 1024;

        private const long Gigabyte = Megabyte * 1024;

        private const long Terabyte = Gigabyte * 1024;

        private const long Petabyte = Terabyte * 1024;

        private readonly long bytes;
        public DataSize(long bytes)
        {
            this.bytes = bytes;
        }

        public static DataSize FromBytes(long bytes)
        {
            return new DataSize(bytes);
        }

        public static DataSize FromBytes(double bytes)
        {
            return new DataSize((long) bytes);
        }

        public static DataSize FromKilobytes(double kilobytes)
        {
            return new DataSize((long)(kilobytes * Kilobyte));
        }

        public static DataSize FromMegabytes(double megabytes)
        {
            return new DataSize((long)(megabytes * Megabyte));
        }

        public static DataSize FromGigabytes(double gigabytes)
        {
            return new DataSize((long)(gigabytes * Gigabyte));
        }

        public static DataSize FromTerabytes(double terabytes)
        {
            return new DataSize((long)(terabytes * Terabyte));
        }

        public static DataSize FromPetabytes(double petabytes)
        {
            return new DataSize((long) (petabytes * Petabyte));
        }

        public long Bytes
        {
            get { return bytes; }
        }

        public double TotalKilobytes
        {
            get { return bytes / (double)Kilobyte; }
        }

        public double TotalMegabytes
        {
            get { return bytes / (double)Megabyte; }
        }

        public double TotalGigabytes
        {
            get { return bytes / (double)Gigabyte; }
        }

        public double TotalTerabytes
        {
            get { return bytes / (double)Terabyte; }
        }

        public double TotalPetabytes
        {
            get { return bytes / (double)Petabyte; }
        }

        public static explicit operator long(DataSize size)
        {
            return size.bytes;
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool shortFormat)
        {
            if (Math.Abs(TotalPetabytes) >= 1)
                return TotalPetabytes.ToString("0.##") + ' ' + (shortFormat ? "PB" : "petabytes");

            if (Math.Abs(TotalTerabytes) >= 1)
                return TotalTerabytes.ToString("0.##") + ' ' + (shortFormat ? "TB" : "terabytes");

            if (Math.Abs(TotalGigabytes) >= 1)
                return TotalGigabytes.ToString("0.##") + ' ' + (shortFormat ? "GB" : "gigabytes");

            if (Math.Abs(TotalMegabytes) >= 1)
                return TotalMegabytes.ToString("0.##") + ' ' + (shortFormat ? "MB" : "megabytes");

            if (Math.Abs(TotalKilobytes) >= 1)
                return TotalKilobytes.ToString("0.##") + ' ' + (shortFormat ? "KB" : "kilobytes");

            return bytes.ToString() + ' ' + (shortFormat ? "B" : "bytes");
        }

        public static DataSize operator +(DataSize size1, DataSize size2)
        {
            return new DataSize(size1.bytes + size2.bytes);
        }

        public static DataSize operator -(DataSize size1, DataSize size2)
        {
            return new DataSize(size1.bytes - size2.bytes);
        }

        public static DataSize operator *(DataSize size, int multiplier)
        {
            return new DataSize(size.bytes * multiplier);
        }

        public static DataSize operator *(int multiplier, DataSize size)
        {
            return size * multiplier;
        }

        public static DataSize operator *(DataSize size, long multiplier)
        {
            return new DataSize(size.bytes * multiplier);
        }

        public static DataSize operator *(long multiplier, DataSize size)
        {
            return size * multiplier;
        }

        public static DataSize operator *(DataSize size, double multiplier)
        {
            return new DataSize((long)(size.bytes * multiplier));
        }

        public static DataSize operator *(double multiplier, DataSize size)
        {
            return size * multiplier;
        }

        public static DataSize operator /(DataSize size, int multiplier)
        {
            return new DataSize(size.bytes / multiplier);
        }

        public static DataSize operator /(DataSize size, long multiplier)
        {
            return new DataSize(size.bytes / multiplier);
        }

        public static DataSize operator /(DataSize size, double multiplier)
        {
            return new DataSize((long)(size.bytes / multiplier));
        }

        public static double operator /(DataSize size1, DataSize size2)
        {
            return size1.bytes / (double) size2.bytes;
        }

        public static DataSize operator -(DataSize size)
        {
            return new DataSize(-size.bytes);
        }

        public bool Equals(DataSize other)
        {
            return bytes == other.bytes;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is DataSize && Equals((DataSize)obj);
        }

        public override int GetHashCode()
        {
            return bytes.GetHashCode();
        }

        public static bool operator ==(DataSize left, DataSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DataSize left, DataSize right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(DataSize other)
        {
            return bytes.CompareTo(other.bytes);
        }

        public static bool operator >(DataSize size1, DataSize size2)
        {
            return size1.bytes > size2.bytes;
        }

        public static bool operator >=(DataSize size1, DataSize size2)
        {
            return size1.bytes >= size2.bytes;
        }

        public static bool operator <(DataSize size1, DataSize size2)
        {
            return size1.bytes < size2.bytes;
        }

        public static bool operator <=(DataSize size1, DataSize size2)
        {
            return size1.bytes <= size2.bytes;
        }
    }
}