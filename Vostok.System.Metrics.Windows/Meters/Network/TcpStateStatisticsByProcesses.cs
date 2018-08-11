﻿using System.Collections.Generic;

namespace Vostok.System.Metrics.Windows.Meters.Network
{
    public struct TcpStateStatisticsByProcesses
    {
        public TcpStateStatisticsByProcesses(Dictionary<int, TcpStateStatistics> statisticsByProcessId)
        {
            StatisticsByProcessId = statisticsByProcessId;
        }

        public Dictionary<int, TcpStateStatistics> StatisticsByProcessId { get; }
    }
}