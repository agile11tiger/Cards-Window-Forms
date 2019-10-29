using DurakLibrary.Common;
using System;
using System.IO;
using System.Net.Sockets;

namespace MainServer
{
    internal class ClientServer
    {
        public BinaryWriter ClientWriter { get; private set; }

        public ClientServer(TcpClient tcpClient, MainClientServer mainClientServer)
        {
            client = tcpClient;
            clientID = Guid.NewGuid().ToString();
            this.mainClientServer = mainClientServer;
            mainClientServer.AddConnectionClient(clientID, this);
        }

        public void HandlingMessagesClient()
        {
            clientStream = client.GetStream();
            clientReader = new BinaryReader(clientStream);
            ClientWriter = new BinaryWriter(clientStream);
            var block = true;

            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)clientReader.ReadByte();

                    switch (netMessageType)
                    {
                        case NetMessageType.DataHosts:
                            MainHostServer.RequestDataAboutHosts(clientID);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    block = false;
                    CloseClient();
                }
            }
        }

        public void CloseClient()
        {
            mainClientServer.RemoveConnectionClient(clientID);
            clientStream?.Close();
            client?.Close();
        }

        private TcpClient client;
        private readonly string clientID;
        private NetworkStream clientStream;
        private MainClientServer mainClientServer;
        private BinaryReader clientReader;
    }
}
