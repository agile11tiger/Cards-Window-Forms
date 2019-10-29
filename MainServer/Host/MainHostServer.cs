using DurakLibrary.Common;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MainServer
{
    internal class MainHostServer
    {
        public void GettingHosts()
        {
            CreatePorts();

            try
            {
                tcpListenerHosts = new TcpListener(IPAddress.Any, NetSettings.PORT_FOR_HOSTS);
                tcpListenerHosts.Start();
                Console.WriteLine("The main Host-Server is running. Waiting for connections...");

                while (true)
                {
                    var tcpClient = tcpListenerHosts.AcceptTcpClient();

                    var result = ports.TryPop(out int port);
                    if (!result) continue;

                    var hostServer = new HostServer(tcpClient, port, this);
                    Task.Run(new Action(hostServer.HandlingMessagesHost));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nException in MainHostServer!\nTargetSite:{ex.TargetSite}\n {ex.Message}\n StackTrace:{ex.StackTrace}");
                DisconnectHosts();
            }
        }

        public void AddConnectionHost(string id, int port, HostServer hostServer)
        {
            if (hosts.TryAdd(id, hostServer))
                Console.WriteLine($"Host-Server is connected: Port = {port}, ID = {id}");
            else
                Console.WriteLine($"Host-Server with such ID = {id} and Port = {port} is already connected!");
        }

        public void RemoveConnectionHost(string id, int port)
        {
            if (hosts.TryRemove(id, out _))
            {
                ports.Push(port);
                Console.WriteLine($"Host-Server is deleted: Port = {port}, ID = {id}");
            }
            else
                Console.WriteLine($"Host-Server with such ID = {id} and Port = {port} doesn`t exist!");
        }

        public static void RequestDataAboutHosts(string clientID)
        {
            foreach (var host in hosts.Values.Where(h => h.IsVisible))
            {
                host.HostWriter.Write((byte)NetMessageType.DataHosts);
                host.HostWriter.Write(clientID);
            }
        }

        public void DisconnectHosts()
        {
            tcpListenerHosts.Stop();

            foreach (var host in hosts.Values)
                host.CloseHost();

            Environment.Exit(0);
        }

        private static ConcurrentDictionary<string, HostServer> hosts = new ConcurrentDictionary<string, HostServer>();
        private static ConcurrentStack<int> ports = new ConcurrentStack<int>();
        private TcpListener tcpListenerHosts;

        private void CreatePorts()
        {
            for (int i = 111; i < 888; i++)
            {
                ports.Push(i);
            }
        }
    }
}
