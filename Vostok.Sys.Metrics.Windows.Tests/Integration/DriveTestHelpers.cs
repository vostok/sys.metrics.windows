using System.Linq;
using Vostok.Sys.Metrics.Windows.Meters.Disk;

namespace Vostok.Sys.Metrics.Windows.IntegrationTests
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