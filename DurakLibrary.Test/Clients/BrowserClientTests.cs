using DurakLibrary.Clients;
using DurakLibrary.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace DurakLibrary.Test.Clients
{
    class BrowserClientTests
    {
        [SetUp]
        public void SetUp()
        {
            Task.Factory.StartNew(GettingClients);
        }

        [Test]
        public void SetBrowserTcp_WithoutException()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var port = NetSettings.PORT_FOR_CLIENTS;
        }

        private bool shouldStop;

        private void GettingClients()
        {
            var tcpListenerClients = new TcpListener(IPAddress.Any, NetSettings.PORT_FOR_CLIENTS);
            tcpListenerClients.Start();

            while(!shouldStop)
            {
                var tcpClient = tcpListenerClients.AcceptTcpClient();
            }
        }
    }
}
