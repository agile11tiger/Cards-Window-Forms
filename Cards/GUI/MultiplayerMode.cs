using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class MultiplayerMode : Form
    {
        public MultiplayerMode()
        {
            InitializeComponent();
        }

        private void ButtonMouseLeft(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.DarkGreen;
        }

        private void ButtonMouseEntered(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Yellow;
        }

        private void BackClick(object sender, EventArgs e)
        {
            Close();
        }
        
        private void HostClick(object sender, EventArgs e)
        {
            Hide();

            try
            {
                var lobby = new Lobby();
                lobby.InitMultiplayer();
                lobby.ShowDialog();
                Show();
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"SocketException: Run the MainServer project in a separate process!\n{ex.Message}", "Error");
            }
        }

        private void JoinClick(object sender, EventArgs e)
        {
            Hide();

            try
            {
                var browser = new Browser();
                browser.ShowDialog();
                Show();
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"SocketException: Run the MainServer project in a separate process!\n{ex.Message}", "Error");
            }
        }
    }
}
