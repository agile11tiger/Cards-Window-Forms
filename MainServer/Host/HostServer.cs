using DurakLibrary.Common;
using System;
using System.IO;
using System.Net.Sockets;

namespace MainServer
{
    internal class HostServer
    {
        public BinaryWriter HostWriter { get; private set; }
        public bool IsVisible { get; private set; } = true;

        public HostServer(TcpClient tcpClient, int port, MainHostServer mainHostServer)
        {
            host = tcpClient;
            hostID = Guid.NewGuid().ToString();
            hostPort = port;
            this.mainHostServer = mainHostServer;
            mainHostServer.AddConnectionHost(hostID, port, this);
        }

        public void HandlingMessagesHost()
        {
            hostStream = host.GetStream();
            hostReader = new BinaryReader(hostStream);
            HostWriter = new BinaryWriter(hostStream);

            HostWriter.Write((byte)NetMessageType.PlayingPort);
            HostWriter.Write(hostPort);
            var block = true;

            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)hostReader.ReadByte();

                    switch (netMessageType)
                    {
                        case NetMessageType.DataHosts:
                            ReadAndSendDataHost();
                            break;
                        case NetMessageType.HostVisibility:
                            IsVisible = hostReader.ReadBoolean();
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

        public void CloseHost()
        {
            mainHostServer.RemoveConnectionHost(hostID, hostPort);
            hostStream?.Close();
            host?.Close();
        }

        private TcpClient host;
        private readonly int hostPort;
        private readonly string hostID;
        private NetworkStream hostStream;
        private MainHostServer mainHostServer;
        private BinaryReader hostReader;

        private void ReadAndSendDataHost()
        {
            var serverTag = new ServerTag();
            serverTag.ReadFromPacket(hostReader);
            MainClientServer.SendDataAboutHost(serverTag);
        }
    }
}
