namespace Vostok.System.Metrics.Windows.Native.Constants
{
    internal enum MIB_TCP_STATE
    {
        MIB_TCP_STATE_CLOSED = 1,
        MIB_TCP_STATE_LISTEN = 2,
        MIB_TCP_STATE_SYN_SENT = 3,
        MIB_TCP_STATE_SYN_RCVD = 4,
        MIB_TCP_STATE_ESTAB = 5,
        MIB_TCP_STATE_FIN_WAIT1 = 6,
        MIB_TCP_STATE_FIN_WAIT2 = 7,
        MIB_TCP_STATE_CLOSE_WAIT = 8,
        MIB_TCP_STATE_CLOSING = 9,
        MIB_TCP_STATE_LAST_ACK = 10,
        MIB_TCP_STATE_TIME_WAIT = 11,
        MIB_TCP_STATE_DELETE_TCB = 12
    }
}