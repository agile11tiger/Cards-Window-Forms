using System;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class Settings : Form
    {
        private bool changesMade = false;

        public Settings()
        {
            InitializeComponent();
            LoadSettingsView();
        }
        
        private void LoadSettingsView()
        {
            txtPlayerName.Text = Properties.Settings.Default.UserName;
            txtServerName.Text = Properties.Settings.Default.DefaultServerName;
            txtServerDescription.Text = Properties.Settings.Default.DefaultServerDescription;
            txtServerPassword.Text = Properties.Settings.Default.DefaultServerPassword;
            trkBotDifficulty.Value = (int)(Properties.Settings.Default.DefaultBotDifficulty * 100);
            chkSimulateBotThink.Checked = Properties.Settings.Default.DefaultBotsThink;
            trkNumPlayers.Value = Properties.Settings.Default.DefaultMaxPlayers;
            changesMade = false;
        }
        
        private bool ApplySettingsView()
        {
            if (VerifyNotEmpty(txtPlayerName, "Player name"))
                Properties.Settings.Default.UserName = txtPlayerName.Text;
            else
                return false;

            if (VerifyNotEmpty(txtServerName, "Server name"))
                Properties.Settings.Default.DefaultServerName = txtServerName.Text;
            else
                return false;

            if (VerifyNotEmpty(txtServerDescription, "Server description"))
                Properties.Settings.Default.DefaultServerDescription = txtServerDescription.Text;
            else
                return false;
            
            Properties.Settings.Default.DefaultServerPassword = txtServerPassword.Text;
            Properties.Settings.Default.DefaultBotDifficulty = trkBotDifficulty.Value / 100.0f;
            Properties.Settings.Default.DefaultBotsThink = chkSimulateBotThink.Checked;
            Properties.Settings.Default.DefaultMaxPlayers = trkNumPlayers.Value;

            return true;
        }
        
        private bool VerifyNotEmpty(TextBox textBox, string paramName)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                MessageBox.Show(paramName + " cannot be empty", "Error", MessageBoxButtons.OK);
                return false;
            }
            else
                return true;
        }
        
        private void InputValueChanged(object sender, EventArgs e)
        {
            changesMade = true;
        }
        
        private void btnSetDefaults_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            changesMade = true;
            LoadSettingsView();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (changesMade)
            {
                var result = MessageBox.Show("You have unsaved changes, are you sure you want to leave?", "Confirm", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    Close();
            }
            else
                Close();
            
        }
        
        private void btnApply_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to apply these settings?", "Confirm", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ApplySettingsView();
                Properties.Settings.Default.Save();
                Close();
            }
        }
        
        private void NumPlayersTRackUpdated(object sender, EventArgs e)
        {
            InputValueChanged(sender, e);
            lblNumPlayers.Text = trkNumPlayers.Value.ToString();
        }
    }
}
