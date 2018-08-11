using System;
using System.IO;
using System.Linq;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
{
    public static class DriveHelpers
    {
        public static DriveInfo FindDrive(char driveLetter)
            => FindDrive(driveLetter.ToString());
        
        public static DriveInfo FindDrive(string driveLetter)
        {
            // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
            var matchingDrive = DriveInfo
                .GetDrives()
                .Where(drive => drive.IsReady)
                .Where(drive => drive.DriveType == DriveType.Fixed)
                .Where(drive => drive.RootDirectory.FullName.StartsWith(driveLetter, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (matchingDrive == null)
                throw new DriveNotFoundException($"Failed to find a ready hard drive with letter '{driveLetter}'.");

            return matchingDrive;
        }

        public static DriveInfo[] GetReadyHardDrives()
        {
            return DriveInfo
                .GetDrives()
                .Where(drive => drive.IsReady)
                .Where(drive => drive.DriveType == DriveType.Fixed)
                .ToArray();
        }

        public static string GetLetter(this DriveInfo drive)
        {
            var name = drive.Name;
            var volumeSeparatorPosition = name.IndexOf(Path.VolumeSeparatorChar);
            return name.Remove(volumeSeparatorPosition);
        }
    }
}