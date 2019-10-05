using DurakLibrary.Common;
using System;
using System.IO;
using System.Net.Sockets;

namespace MainServer
{
    public class HostServer
    {
        private TcpClient host;
        private readonly int hostPort;
        private readonly string hostID;
        private NetworkStream hostStream;
        private MainHostServer mainHostServer;
        protected internal BinaryReader HostReader { get; private set; }
        protected internal BinaryWriter HostWriter { get; private set; }
        protected internal bool IsVisible { get; private set; } = true;

        public HostServer(TcpClient tcpClient, int port, MainHostServer mainHostServer)
        {
            host = tcpClient;
            hostID = Guid.NewGuid().ToString();
            hostPort = port;
            this.mainHostServer = mainHostServer;
            mainHostServer.AddConnectionHost(hostID, port, this);
        }

        protected internal void HandlingMessagesHost()
        {
            hostStream = host.GetStream();
            HostReader = new BinaryReader(hostStream);
            HostWriter = new BinaryWriter(hostStream);

            HostWriter.Write((byte)NetMessageType.PlayingPort);
            HostWriter.Write(hostPort);
            var block = true;

            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)HostReader.ReadByte();

                    switch (netMessageType)
                    {
                        case NetMessageType.DataHosts:
                            ReadAndSendDataHost();
                            break;
                        case NetMessageType.HostVisibility:
                            IsVisible = HostReader.ReadBoolean();
                            break;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    block = false;
                    CloseHost();
                }
            }
        }

        protected internal void CloseHost()
        {
            mainHostServer.RemoveConnectionHost(hostID, hostPort);
            hostStream?.Close();
            host?.Close();
        }

        private void ReadAndSendDataHost()
        {
            var serverTag = new ServerTag();
            serverTag.ReadFromPacket(HostReader);
            MainClientServer.SendDataAboutHost(serverTag);
        }
    }
}
