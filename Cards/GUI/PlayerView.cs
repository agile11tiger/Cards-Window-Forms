using System;
using System.Windows.Forms;
using DurakLibrary.Common;
using DurakGame.Properties;
using DurakLibrary.Clients;
using DurakLibrary.HostServer;

namespace DurakGame
{
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
        }

        public bool HasControl
        {
            get => hasControl;
            set
            {
                if (hasControl != value)
                {
                    hasControl = value;

                    if (hasControl)
                        imgReady.Click += ReadyClicked;
                    else
                        imgReady.Click -= ReadyClicked;
                }
            }
        }

        public bool IsReady
        {
            get => player.IsReady;
            set
            {
                player.IsReady = value;
                DetermineReadyImage();
            }
        }

        public Player Player
        {
            get => player;
            set
            {
                player = value;

                if (player != null)
                {
                    if (singleClient != null && player.IsBot)
                    {
                        imgPlayerType.Image = Resources.bot;

                        if (singleClient.Player.IsHost)
                            imgReady.Click += ReadyClicked;
                    }
                    else if (multiClient != null && player.ID != multiClient.PlayerUntill.ID)
                        imgPlayerType.Image = Resources.netIcon;
                    else
                        imgPlayerType.Image = Resources.silhoutte;

                    lblPlayerName.Text = player.Name;
                    DetermineReadyImage();
                }
            }
        }

        public SingleClientServer SingleClient
        {
            get => singleClient;
            set
            {
                singleClient = value;
                cmsContextMenu.Enabled = false;
            }
        }

        public LobbyClient MultiClient
        {
            get => multiClient;
            set
            {
                multiClient = value;

                if (multiClient != null)
                    cmsContextMenu.Enabled = multiClient.PlayerUntill.IsHost;
            }
        }

        public void SetReadiness(bool isReady)
        {
            player.IsReady = isReady;

            if (isReady)
                imgReady.Image = Resources.ready;
            else
                imgReady.Image = Resources.notReady;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Right)
                cmsContextMenu.Show(this, e.Location, ToolStripDropDownDirection.Default);
        }

        private Player player;
        private SingleClientServer singleClient;
        private LobbyClient multiClient;
        private bool hasControl;

        private void ReadyClicked(object sender, EventArgs e)
        {
            if (singleClient != null && player.IsBot && singleClient.Player.IsHost)
                singleClient.RemoveBot(player);

            if (multiClient != null && HasControl && !Player.IsHost)
                multiClient.ReadyClicked(!player.IsReady);
        }

        private void DetermineReadyImage()
        {
            if (singleClient != null && Player.IsBot)
                imgReady.Image = singleClient.Player.IsHost ? Resources.delete : null;
            else if (!Player.IsHost)
                imgReady.Image = IsReady ? Resources.ready : Resources.notReady;
            else
                imgReady.Image = null;
        }

        private void KickPlayerPressed(object sender, EventArgs e)
        {
            var result = DialogResult.Yes;

            if (!Player.IsBot)
                result = MessageBox.Show(string.Format("Are you sure you want to kick {0}?", Player.Name), "Confirm", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
                multiClient.RequestKick(Player);
        }
    }
}
