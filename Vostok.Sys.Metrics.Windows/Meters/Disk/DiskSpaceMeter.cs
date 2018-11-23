using System;
using System.IO;

namespace Vostok.Sys.Metrics.Windows.Meters.Disk
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
        public long GetTotalSpaceBytes()
        {
            return drive.TotalSize;
        }

        /// <summary>
        /// Returns amount of free space on disk.
        /// </summary>
        public long GetFreeSpaceBytes()
        {
            return drive.TotalFreeSpace;
        }

        public void Dispose()
        {
        }

        private readonly DriveInfo drive;
    }
}