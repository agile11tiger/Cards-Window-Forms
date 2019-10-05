using DurakLibrary.Common;
using System;
using System.IO;
using System.Net.Sockets;

namespace MainServer
{
    public class ClientServer
    {
        private TcpClient client;
        private readonly string clientID;
        private NetworkStream clientStream;
        private MainClientServer mainClientServer;
        protected internal BinaryReader ClientReader { get; private set; }
        protected internal BinaryWriter ClientWriter { get; private set; }

        public ClientServer(TcpClient tcpClient, MainClientServer mainClientServer)
        {
            client = tcpClient;
            clientID = Guid.NewGuid().ToString();
            this.mainClientServer = mainClientServer;
            mainClientServer.AddConnectionClient(clientID, this);
        }

        protected internal void HandlingMessagesClient()
        {
            clientStream = client.GetStream();
            ClientReader = new BinaryReader(clientStream);
            ClientWriter = new BinaryWriter(clientStream);
            var block = true;

            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)ClientReader.ReadByte();

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

        protected internal void CloseClient()
        {
            mainClientServer.RemoveConnectionClient(clientID);
            clientStream?.Close();
            client?.Close();
        }
    }
}
