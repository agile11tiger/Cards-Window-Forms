using System;
using System.Drawing;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class Main : Form
    {
        public Main()
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
        
        private void btnPlaySingle_Click(object sender, EventArgs e)
        {
            Hide();
            var lobby = new Lobby();
            lobby.InitSinglePlayer();
            lobby.ShowDialog();
            Show();
        }

        private void btnPlayMulti_Click(object sender, EventArgs e)
        {
            Hide();
            new MultiplayerMode().ShowDialog();
            Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Hide();
            new Settings().ShowDialog();
            Show();
        }
    }
}
