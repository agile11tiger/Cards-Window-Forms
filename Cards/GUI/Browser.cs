using DurakLibrary.Clients;
using DurakLibrary.Common;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class Browser : Form
    {
        public Browser()
        {
            InitializeComponent();
            Initialize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            timer.Stop();
            base.OnClosing(e);
            browserClient.IsLobbyClosing = true;
            browserClient.CloseLobby("Disconnected");
            browserClient.IsBrowserClosing = true;
            browserClient.CloseBrowser(null);
        }

        private Timer timer;
        private BrowserClient browserClient;

        private void Initialize()
        {
            InitClient();
        }

        private void AddEvents()
        {
            browserClient.OnHostDiscovered += UpdateHosts;
            browserClient.OnConnected += CreateLobby;
            browserClient.OnBrowserClose += Close;
            dgvServers.MouseDoubleClick += ServerListDoubleClicked;
        }

        private void InitClient()
        {
            var player = new Player(Properties.Settings.Default.UserName) { IPAddress = NetUtils.GetAddress() };
            browserClient = new BrowserClient(player);
            browserClient.SetBrowserTcp(player.IPAddress, NetSettings.PORT_FOR_CLIENTS);
            browserClient.RunBrowserClient();

            AddEvents();
            browserClient.RequestDataAboutHosts();

            timer = new Timer();
            timer.Interval = 10000;
            timer.Enabled = true;
            timer.Tick += btnRefresh_Click;
            timer.Start();
        }
        
        private void ServerListDoubleClicked(object sender, MouseEventArgs e)
        {
            var row = dgvServers.HitTest(e.X, e.Y).RowIndex;

            if (row >= 0 && row < dgvServers.RowCount)
            {
                DataGridViewRow dataRow = dgvServers.Rows[row];

                if (dataRow != null)
                {
                    var tag = (ServerTag)dataRow.Tag;

                    if (tag.PasswordProtected)
                    {
                        var password = Prompt.ShowDialog("Enter password", "Enter Password");

                        if (tag.Password == password)
                            browserClient.ConnectTo(tag);
                        else
                        {
                            MessageBox.Show("Incorrect password");
                            return;
                        }
                    }
                    else
                        browserClient.ConnectTo(tag);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            browserClient.RequestDataAboutHosts();
            dgvServers.Rows.Clear();
        }

        private void UpdateHosts(ServerTag tag)
        {
            var row = new DataGridViewRow();
            row.Tag = tag;
            row.CreateCells(dgvServers);
            row.Cells[0].Value = tag.Name;
            row.Cells[1].Value = tag.PlayersCount + "/" + tag.SupportedPlayerCount;

            dgvServers.Rows.Add(row);
        }

        private void CreateLobby()
        {
            timer.Stop();
            Hide();

            var lobby = new Lobby();
            lobby.InitMultiplayer(browserClient);
            lobby.ShowDialog();

            Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
