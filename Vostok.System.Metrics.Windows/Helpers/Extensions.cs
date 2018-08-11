using System.Globalization;

namespace Vostok.System.Metrics.Windows.Helpers
{
    internal static class Extensions
    {
        public static string Format(this double d) => d.ToString("F2", CultureInfo.InvariantCulture);
    }
}