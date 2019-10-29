using System;
using System.Linq;
using System.Net;

namespace DurakLibrary.Common
{
    public static class NetUtils
    {
        public static IPAddress GetAddress()
        {
            return Dns.GetHostEntry(Environment.MachineName)
                .AddressList.Where(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .FirstOrDefault();
        }
    }
}
