using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Vostok.Sys.Metrics.Windows.Meters.Network
{
    public class TcpStateStatistics
    {
        public int Total => Closed + Listen + SynSent + SynRcvd + Established + FinWait1 + FinWait2 + CloseWait +
                            Closing + LastAck + TimeWait + DeleteTcb + Unknown;
        
        public int Unknown { get; internal set; }
        public int Closed { get; internal set; }
        public int Listen { get; internal set; }
        public int SynSent { get; internal set; }
        public int SynRcvd { get; internal set; }
        public int Established { get; internal set; }
        public int FinWait1 { get; internal set; }
        public int FinWait2 { get; internal set; }
        public int CloseWait { get; internal set; }
        public int Closing { get; internal set; }
        public int LastAck { get; internal set; }
        public int TimeWait { get; internal set; }
        public int DeleteTcb { get; internal set; }
        
        public override string ToString()
            => $"Total: {Total}, Closed: {Closed}, Listen: {Listen}, SynSent: {SynSent}, SynRcvd: {SynRcvd}, Established: {Established}, FinWait1: {FinWait1}, FinWait2: {FinWait2}, "
             + $"CloseWait: {CloseWait}, Closing: {Closing}, LastAck: {LastAck}, TimeWait: {TimeWait}, DeleteTcb: {DeleteTcb}";

        public Dictionary<TcpState, int> ToDictionary() => new Dictionary<TcpState, int>
        {
            {TcpState.Closed, Closed},
            {TcpState.CloseWait, CloseWait},
            {TcpState.Closing, Closing},
            {TcpState.DeleteTcb, DeleteTcb},
            {TcpState.Established, Established},
            {TcpState.FinWait1, FinWait1},
            {TcpState.FinWait2, FinWait2},
            {TcpState.LastAck, LastAck},
            {TcpState.Listen, Listen},
            {TcpState.SynReceived, SynRcvd},
            {TcpState.SynSent, SynSent},
            {TcpState.TimeWait, TimeWait},
            {TcpState.Unknown, Unknown}
        };
        
        public static TcpStateStatistics operator +(TcpStateStatistics a, TcpStateStatistics b)
            => new TcpStateStatistics
            {
                Closed = a.Closed + b.Closed,
                CloseWait = a.CloseWait + b.CloseWait,
                Closing = a.Closing + b.Closing,
                DeleteTcb = a.DeleteTcb + b.DeleteTcb,
                Established = a.Established + b.Established,
                FinWait1 = a.FinWait1 + b.FinWait1,
                FinWait2 = a.FinWait2 + b.FinWait2,
                LastAck = a.LastAck + b.LastAck,
                Listen = a.Listen + b.Listen,
                SynRcvd = a.SynRcvd + b.SynRcvd,
                SynSent = a.SynSent + b.SynSent,
                TimeWait = a.TimeWait + b.TimeWait,
                Unknown = a.Unknown + b.Unknown
            };
    }
}