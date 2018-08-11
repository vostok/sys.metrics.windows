using System;
using System.IO;
using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.Meters.Disk
{
    public class DiskSpaceMeter : IDisposable
    {
        public DiskSpaceMeter(string driveLetter)
        {
            drive = DriveHelpers.FindDrive(driveLetter);
        }

        public DiskSpaceMeter(char driveLetter)
            : this(driveLetter.ToString()) { }

        /// <summary>
        /// Returns amount of total space on disk.
        /// </summary>
        public DataSize GetTotalSpace()
        {
            return DataSize.FromBytes(drive.TotalSize);
        }

        /// <summary>
        /// Returns amount of free space on disk.
        /// </summary>
        public DataSize GetFreeSpace()
        {
            return DataSize.FromBytes(drive.TotalFreeSpace);
        }

        public void Dispose()
        {
        }

        private readonly DriveInfo drive;
    }
}