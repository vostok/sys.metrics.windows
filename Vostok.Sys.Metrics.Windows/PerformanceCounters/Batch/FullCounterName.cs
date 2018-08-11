using System;

namespace Vostok.Sys.Metrics.Windows.PerformanceCounters.Batch
{
    internal struct FullCounterName : IEquatable<FullCounterName>
    {
        public FullCounterName(string categoryName, string counterName)
        {
            CategoryName = categoryName;
            CounterName = counterName;
        }

        public string CategoryName { get; }
        public string CounterName { get; }

        public bool Equals(FullCounterName other)
        {
            return string.Equals(CategoryName, other.CategoryName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(CounterName, other.CounterName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is FullCounterName info && Equals(info);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((CategoryName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(CategoryName) : 0) * 397) ^
                       (CounterName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(CounterName) : 0);
            }
        }

        public static bool operator ==(FullCounterName left, FullCounterName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FullCounterName left, FullCounterName right)
        {
            return !left.Equals(right);
        }
    }
}