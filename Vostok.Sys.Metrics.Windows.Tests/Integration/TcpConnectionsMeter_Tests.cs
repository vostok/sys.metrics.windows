using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using FluentAssertions;
using Vostok.Sys.Metrics.Windows.Meters.Network;
using NUnit.Framework;

namespace Vostok.Sys.Metrics.Windows.IntegrationTests
{
    public class TcpConnectionsMeter_Tests
    {
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(200)]
        public void ForPort(int connectionsCount)
        {
            using (var listener = new Listener(IPAddress.Loopback))
            {
                using (var meter = new PortTcpConnectionsMeter(listener.EndPoint.Port))
                {
                    var before = meter.GetStatistics().Total;
                    var clients = new List<TcpClient>();
                    try
                    {
                        for (var i = 0; i < connectionsCount; ++i)
                        {
                            var client = new TcpClient();
                            clients.Add(client);
                            client.Connect(listener.EndPoint);
                        }

                        var after = meter.GetStatistics().Total;
                        (after - before).Should().Be(connectionsCount);
                    }
                    finally
                    {
                        clients.ForEach(x => x.Dispose());
                    }
                }
            }
        }

        [TestCase(10)]
        [TestCase(100)]
        public void ForPort_IpV6(int connectionsCount)
        {
            using (var listener = new Listener(IPAddress.IPv6Loopback))
            {
                using (var meter = new PortTcpConnectionsMeter(listener.EndPoint.Port))
                {
                    var before = meter.GetStatisticsByTCPv6().Total;
                    var clients = new List<TcpClient>();
                    try
                    {
                        for (var i = 0; i < connectionsCount; ++i)
                        {
                            var client = new TcpClient(AddressFamily.InterNetworkV6);
                            clients.Add(client);
                            client.Connect(listener.EndPoint);
                        }

                        var after = meter.GetStatisticsByTCPv6().Total;
                        (after - before).Should().Be(connectionsCount);
                    }
                    finally
                    {
                        clients.ForEach(x => x.Dispose());
                    }
                }
            }
        }
    }

    public class Listener : IDisposable
    {
        private TcpListener tcpListener;

        public Listener(IPAddress ipAddress)
        {
            tcpListener = new TcpListener(ipAddress, 0);
            tcpListener.Start();
            
        }
    
        public IPEndPoint EndPoint => (IPEndPoint) tcpListener.LocalEndpoint;

        public void Dispose() => tcpListener?.Stop();
    }
}