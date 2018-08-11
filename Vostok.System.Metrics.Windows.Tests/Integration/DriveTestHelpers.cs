using System.Linq;
using Vostok.System.Metrics.Windows.Meters.Disk;

namespace Vostok.System.Metrics.Windows.IntegrationTests
{
    public static class DriveTestHelpers
    {
        public static string GetDriveLetter()
        {
            return DriveHelpers
                .GetReadyHardDrives()
                .First()
                .GetLetter();
        }
    }
}