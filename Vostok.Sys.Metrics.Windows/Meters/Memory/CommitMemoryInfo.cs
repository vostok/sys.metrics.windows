﻿using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.Memory
{
    public struct CommitMemoryInfo
    {
        public long CommittedBytes;
        public long LimitBytes;

        public double UsageBytes => CommittedBytes / (double) LimitBytes;
        
        public override string ToString()
        {
            return $"Committed = {new DataSize(CommittedBytes)}/{new DataSize(LimitBytes)}";
        }
    }
}