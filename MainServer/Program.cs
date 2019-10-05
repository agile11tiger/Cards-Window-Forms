using System;
using System.Threading.Tasks;

namespace MainServer
{
    class Program
    {
        private static MainHostServer serverHosts;
        private static MainClientServer serverClients;

        static void Main(string[] args)
        {
            try
            {
                serverHosts = new MainHostServer();
                serverClients = new MainClientServer();
                Task.Factory.StartNew(serverClients.GettingClients, TaskCreationOptions.LongRunning);
                Task.Factory.StartNew(serverHosts.GettingHosts, TaskCreationOptions.LongRunning);

                while(true)
                {
                    Console.WriteLine("If you want to disconnect the main Server, press '7'!");
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '7') break;
                }
            }
            catch (Exception ex)
            {
                serverHosts.DisconnectHosts();
                serverClients.DisconnectClients();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
