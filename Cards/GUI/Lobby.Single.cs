using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DurakGame
{
    partial class Lobby
    {
        public void InitSinglePlayer()
        {
            var player = new Player(Properties.Settings.Default.UserName, START_ID, true) { IPAddress = NetUtils.GetAddress() };
            singleServer = new SingleClientServer(player);
            singleServer.IsSinglePlayerMode = true;
            singleServer.ConnectedServer = new ServerTag(
                Properties.Settings.Default.DefaultServerName,
                Properties.Settings.Default.DefaultServerDescription,
                Properties.Settings.Default.DefaultServerPassword, 
                Properties.Settings.Default.DefaultMaxPlayers);
            singleServer.CreatePlayersID();
            singleServer.ConnectedServer.AddPlayer(player);
            singleServer.ConnectedServer.State = ServerState.InLobby;
            singleServer.OnBotAdded += AddPlayer;
            singleServer.OnBotRemoved += RemovePlayer;

            AddPlayer(player);
        }

        private SingleClientServer singleServer;

        private void btnAddBot_Click(object sender, EventArgs e)
        {
            singleServer?.AddBot(Properties.Settings.Default.DefaultBotDifficulty, txtBotName.Text);
        }

        private void StartSingleMode()
        {
            if (singleServer.Player.IsHost)
            {
                if (singleServer.ConnectedServer == null)
                    MessageBox.Show("Error, client not connected to local server");
                else
                {
                    if (views.Count > 1)
                    {
                        var numCards = rbn20Cards.Checked ? 20 : rbn52Cards.Checked ? 52 : 36;

                        if (numCards / views.Count >= 6)
                        {
                            var parameter = StateParameter.Construct(Names.AMOUNT_INIT_CARDS, numCards, true);
                            singleServer.GameState.Set(Names.AMOUNT_INIT_CARDS, parameter.GetValueInt(), true);
                            singleServer.SetServerState(ServerState.InGame);
                            singleServer.SetBotSettings(chkSimulateBotThinkTime.Checked);
                            ServerStateUpdated(this, singleServer.ConnectedServer.State);
                        }
                        else
                            MessageBox.Show("You do not have enough cards for that number of players" +
                                "\nPlease ensure theres at least enough cards for 6 cards per player", "Alert", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("You need at least 2 players to play", "Alert", MessageBoxButtons.OK);
                }
            }
        }

        private void ServerStateUpdated(object sender, ServerState e)
        {
            if (e == ServerState.InGame)
            {
                Hide();
                var mainForm = new Game();
                mainForm.SetClient(singleServer);
                mainForm.AddSingleClientEvents(singleServer);
                singleServer.GameState.UpdateParameters();
                singleServer.RunBots(SynchronizationContext.Current);
                mainForm.ShowDialog();
            }
        }
    }
}
