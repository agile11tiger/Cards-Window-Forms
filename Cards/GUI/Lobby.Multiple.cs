using DurakLibrary.Clients;
using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DurakGame
{
    partial class Lobby
    {
        public void InitMultiplayer()
        {
            gameServer = new MainPlayerServer(Properties.Settings.Default.DefaultMaxPlayers);
            gameServer.Run();
            InitServer();

            CreateConnectedServer(lobbyClient);
            Thread.Sleep(100); //In order for MainPlayerServer.Host.GameClient to be initialized.
            CreateConnectedServer(MainPlayerServer.Host.Core);
            lobbyClient.ConnectHost();

            client = lobbyClient;
            AddEvents();
            CreateVoiceChat();
        }

        public void InitMultiplayer(BrowserClient browserClient)
        {
            this.browserClient = browserClient;
            client = browserClient;
            AddEvents();
            CreateVoiceChat();
        }

        public void SetReadiness(int id, bool isReady)
        {
            views.First(v => v.Player.ID == id).SetReadiness(isReady);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (client != null)
                client.IsLobbyClosing = true;

            gameServer?.DisconnectPlayers();
        }

        private MainPlayerServer gameServer;
        private LobbyClient lobbyClient;
        private BrowserClient browserClient;
        private LobbyClient client;

        private void CreateConnectedServer(CoreDurakGame core)
        {
            core.ConnectedServer = new ServerTag(
                Properties.Settings.Default.DefaultServerName, 
                Properties.Settings.Default.DefaultServerDescription,
                Properties.Settings.Default.DefaultServerPassword, 
                Properties.Settings.Default.DefaultMaxPlayers)
            {
                IPAddress = NetUtils.GetAddress(),
                Port = gameServer.Port,
                State = ServerState.InLobby,
            };
        }

        private void InitServer()
        {
            var player = new Player(Properties.Settings.Default.UserName, START_ID, true, true) { IPAddress = NetUtils.GetAddress() };
            lobbyClient = new LobbyClient(player);
            lobbyClient.SetLobbyTcp(player.IPAddress, gameServer.Port);
            lobbyClient.RunLobbyClient();
        }

        private void AddEvents()
        {
            client.OnPlayerConnected += AddPlayer;
            client.OnPlayerDisconnectedLobby += RemovePlayer;
            client.OnServerStateUpdated += ServerStateUpdated;
            client.OnLobbyClose += Close;
            client.OnPlayerIsReady += SetReadiness;
            client.OnPlayerChatLobby += AddChatMessage;
            client.OnCannotStart += CannotStart;
        }

        private void CreateVoiceChat()
        {
            client.Chat = new VoiceChat(client.ConnectedServer.IPAddress, client.ConnectedServer.Port);
        }

        private void AddChatMessage(int id, string message)
        {
            var view = views.FirstOrDefault(x => x.Player.ID == id);

            if (view != null)
                txtChat.AppendText(string.Format("[{0}]: {1}\n", id == view.MultiClient.Player.ID ? "" : view.Player.Name, message));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMessage.Text))
                client?.SendChatMessage(txtMessage.Text);

            txtMessage.Text = "";
        }

        private void StartMultipleMode()
        {
            if (client.Player.IsHost)
            {
                if (client.ConnectedServer == null)
                    MessageBox.Show("Error, client not connected to local server");
                else
                {
                    if (views.Count > 1)
                    {
                        var numCards = rbn20Cards.Checked ? 20 : rbn52Cards.Checked ? 52 : 36;

                        if (numCards / views.Count >= 6)
                        {
                            client.RequestState(StateParameter.Construct(Names.AMOUNT_INIT_CARDS, numCards, true));
                            client.RequestStart();
                        }
                        else
                            MessageBox.Show("You do not have enough cards for that number of players\nPlease ensure theres at least enough cards for 6 cards per player", "Alert", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("You need at least 2 players to play", "Alert", MessageBoxButtons.OK);
                }
            }
        }

        private void CannotStart(string message)
        {
            MessageBox.Show(message, "Server", MessageBoxButtons.OK);
        }

        private void ServerStateUpdated(ServerState state)
        {
            if (state == ServerState.InGame)
            {
                Hide();
                var mainForm = new Game();
                mainForm.SetClient(client);
                mainForm.AddMultiClientEvents(client);
                mainForm.ShowDialog();
                Show();
            }
            else if (state == ServerState.InLobby)
            {
                client.PrepareToReturnToLobby();
            }
        }

        private void Close(string reason)
        {
            if (reason != null)
                MessageBox.Show(reason, "Alert", MessageBoxButtons.OK);

            Close();
        }
    }
}
