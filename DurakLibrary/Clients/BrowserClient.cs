using DurakLibrary.Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakLibrary.Clients
{
    public class BrowserClient : LobbyClient
    {
        public event Action<ServerTag> OnHostDiscovered;
        public event Action OnBrowserClose;
        public bool IsBrowserClosing;

        public BrowserClient(Player player) : base(player)
        {
        }

        public void SetBrowserTcp(IPAddress ip, int port)
        {
            browserTcp = new TcpClient();
            browserTcp.Connect(ip, port);

            RunBrowserClient();
            CheckConnection(ref browserWriter);
        }

        public void RequestDataAboutHosts()
        {
            browserWriter.Write((byte)NetMessageType.DataHosts);
        }

        public void CloseBrowser(object state)
        {
            if (!IsBrowserClosing)
                OnBrowserClose.Invoke();

            if (browserStream != null)
                browserStream.Close();

            if (browserTcp != null)
                browserTcp.Close();
        }

        private TcpClient browserTcp;
        private NetworkStream browserStream;
        private BinaryReader browserReader;
        private BinaryWriter browserWriter;

        private async void RunBrowserClient()
        {
            var guiContext = SynchronizationContext.Current;
            await Task.Factory.StartNew(() => HandlingMessagesBrowser(guiContext), TaskCreationOptions.LongRunning);
        }

        private void HandlingMessagesBrowser(SynchronizationContext guiContext)
        {
            browserStream = browserTcp.GetStream();
            browserReader = new BinaryReader(browserStream);
            browserWriter = new BinaryWriter(browserStream);
            var block = true;

            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)browserReader.ReadByte();

                    switch (netMessageType)
                    {
                        case NetMessageType.DataHosts:
                            guiContext.Send(ReceivedDataAboutHosts, null);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    block = false;
                    guiContext.Send(CloseBrowser, null);

                    if (!(ex is EndOfStreamException || ex is ObjectDisposedException || ex.HResult == -2146232800))
                        MessageBox.Show($"Exception in BrowserClient!\n {ex.Message}\n StackTrace:{ex.StackTrace}");
                }
            }
        }
        private void ReceivedDataAboutHosts(object obj)
        {
            var serverTag = new ServerTag();
            serverTag.ReadFromPacket(browserReader);
            OnHostDiscovered.Invoke(serverTag);
        }
    }
}
