using DurakLibrary.Cards;
using DurakLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class Lobby : Form
    {
        private const int START_ID = 0;
        private List<PlayerView> views;

        public Lobby()
        {
            InitializeComponent();
            views = new List<PlayerView>();
            DialogResult = DialogResult.OK;
            chkSimulateBotThinkTime.Checked = Properties.Settings.Default.DefaultBotsThink;
        }

        private void RemovePlayer(int id)
        {
            var view = views.FirstOrDefault(x => x.Player.ID == id);

            if (view != null)
            {
                views.Remove(view);
                pnlPlayers.Controls.Remove(view);
                UpdatePlayerView();
            }
        }

        private void AddPlayer(Player player)
        {
            var view = BuildView(player);
            pnlPlayers.Controls.Add(view);
            views.Add(view);
            UpdatePlayerView();

            var needingServer = singleServer != null ? singleServer : (CoreDurakGame)client;

            if (player.ID == needingServer.PlayerUntill.ID)
            {
                lblServerName.Text = needingServer.ConnectedServer.Name;
                lblServerDescription.Text = needingServer.ConnectedServer.Description;
                view.HasControl = true;
            }

            if (client != null && !client.PlayerUntill.IsHost)
                grpGameSettings.Visible = false;
        }

        private void UpdatePlayerView()
        {
            for (int index = 0; index < views.Count; index++)
                views[index].Top = 5 + index * views[index].Height;

            if (singleServer != null)
            {
                if (singleServer.ConnectedServer.PlayersCount == Properties.Settings.Default.DefaultMaxPlayers)
                    pnlAddBot.Visible = false;
                else
                {
                    pnlAddBot.Visible = singleServer.Player.IsHost;
                    pnlAddBot.Top = 5 + views.Count * 60;
                }
            }
            else
                pnlAddBot.Visible = false;
        }

        private PlayerView BuildView(Player player)
        {
            var view = new PlayerView();

            if (singleServer != null)
                view.SingleClient = singleServer;
            else
                view.MultiClient = client;

            view.Player = player;
            view.IsReady = player.IsReady;
            view.Left = 5;

            return view;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (singleServer != null)
                StartSingleMode();
            if (client != null)
                StartMultipleMode();
        }
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
