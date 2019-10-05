using DurakLibrary.Common;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MainServer
{
    public class MainClientServer
    {
        private static ConcurrentDictionary<string, ClientServer> clients = new ConcurrentDictionary<string, ClientServer>();
        private static object locker = new object();
        private TcpListener tcpListenerClients;
        
        protected internal void AddConnectionClient(string id, ClientServer clientServer)
        {
            if (clients.TryAdd(id, clientServer))
                Console.WriteLine($"Client-Server is connected: ID = {id}");
            else
                Console.WriteLine($"Client-Server with such ID = {id} is already connected!");
        }

        protected internal void RemoveConnectionClient(string id)
        {
            if (clients.TryRemove(id, out _))
                Console.WriteLine($"Client-Server is deleted: ID = {id}");
            else
                Console.WriteLine($"Client-Server with such ID = {id} doesn`t exist!");
        }

        public void GettingClients()
        {
            try
            {
                tcpListenerClients = new TcpListener(IPAddress.Any, NetSettings.PORT_FOR_CLIENTS);
                tcpListenerClients.Start();
                Console.WriteLine("The main Client-Server is running. Waiting for connections...");

                while (true)
                {
                    var tcpClient = tcpListenerClients.AcceptTcpClient();

                    var clientServer = new ClientServer(tcpClient, this);
                    Task.Run(new Action(clientServer.HandlingMessagesClient));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nException in MainClientServer!\nTargetSite:{ex.TargetSite}\n {ex.Message}\n StackTrace:{ex.StackTrace}");
                DisconnectClients();
            }
        }

        protected internal static void SendDataAboutHost(ServerTag serverTag)
        {
            lock (locker)
            {
                var client = clients[serverTag.SenderID];
                client.ClientWriter.Write((byte)NetMessageType.DataHosts);
                serverTag.WriteToPacket(client.ClientWriter, serverTag.SenderID);
            }
        }
        
        protected internal void DisconnectClients()
        {
            tcpListenerClients.Stop();

            foreach(var client in clients.Values)
                client.CloseClient();
            
            Environment.Exit(0);
        }
    }
}
