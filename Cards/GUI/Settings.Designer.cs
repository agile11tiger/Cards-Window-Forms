namespace DurakGame
{
    partial class Settings
    {
        private System.ComponentModel.IContainer components = null;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnSetDefaults = new System.Windows.Forms.Button();
            this.grpPlayerSettings = new System.Windows.Forms.GroupBox();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.grpHosting = new System.Windows.Forms.GroupBox();
            this.lblServerPassword = new System.Windows.Forms.Label();
            this.txtServerPassword = new System.Windows.Forms.TextBox();
            this.lblNumPlayers = new System.Windows.Forms.Label();
            this.lblPlayerCount = new System.Windows.Forms.Label();
            this.trkNumPlayers = new System.Windows.Forms.TrackBar();
            this.chkSimulateBotThink = new System.Windows.Forms.CheckBox();
            this.lblBotDifficulty = new System.Windows.Forms.Label();
            this.trkBotDifficulty = new System.Windows.Forms.TrackBar();
            this.txtServerDescription = new System.Windows.Forms.TextBox();
            this.lblServerDescription = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.grpPlayerSettings.SuspendLayout();
            this.grpHosting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkNumPlayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBotDifficulty)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 398);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(307, 398);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Location = new System.Drawing.Point(93, 398);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new System.Drawing.Size(75, 23);
            this.btnSetDefaults.TabIndex = 2;
            this.btnSetDefaults.Text = "Defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new System.EventHandler(this.btnSetDefaults_Click);
            // 
            // grpPlayerSettings
            // 
            this.grpPlayerSettings.Controls.Add(this.txtPlayerName);
            this.grpPlayerSettings.Controls.Add(this.lblName);
            this.grpPlayerSettings.Location = new System.Drawing.Point(12, 13);
            this.grpPlayerSettings.Name = "grpPlayerSettings";
            this.grpPlayerSettings.Size = new System.Drawing.Size(370, 77);
            this.grpPlayerSettings.TabIndex = 3;
            this.grpPlayerSettings.TabStop = false;
            this.grpPlayerSettings.Text = "Player";
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Location = new System.Drawing.Point(81, 17);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(185, 20);
            this.txtPlayerName.TabIndex = 1;
            this.txtPlayerName.TextChanged += new System.EventHandler(this.InputValueChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(37, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpHosting
            // 
            this.grpHosting.Controls.Add(this.lblServerPassword);
            this.grpHosting.Controls.Add(this.txtServerPassword);
            this.grpHosting.Controls.Add(this.lblNumPlayers);
            this.grpHosting.Controls.Add(this.lblPlayerCount);
            this.grpHosting.Controls.Add(this.trkNumPlayers);
            this.grpHosting.Controls.Add(this.chkSimulateBotThink);
            this.grpHosting.Controls.Add(this.lblBotDifficulty);
            this.grpHosting.Controls.Add(this.trkBotDifficulty);
            this.grpHosting.Controls.Add(this.txtServerDescription);
            this.grpHosting.Controls.Add(this.lblServerDescription);
            this.grpHosting.Controls.Add(this.txtServerName);
            this.grpHosting.Controls.Add(this.lblServerName);
            this.grpHosting.Location = new System.Drawing.Point(12, 96);
            this.grpHosting.Name = "grpHosting";
            this.grpHosting.Size = new System.Drawing.Size(370, 296);
            this.grpHosting.TabIndex = 4;
            this.grpHosting.TabStop = false;
            this.grpHosting.Text = "Host Default Options";
            // 
            // lblServerPassword
            // 
            this.lblServerPassword.AutoSize = true;
            this.lblServerPassword.Location = new System.Drawing.Point(6, 139);
            this.lblServerPassword.Name = "lblServerPassword";
            this.lblServerPassword.Size = new System.Drawing.Size(90, 13);
            this.lblServerPassword.TabIndex = 12;
            this.lblServerPassword.Text = "Server Password:";
            // 
            // txtServerPassword
            // 
            this.txtServerPassword.Location = new System.Drawing.Point(100, 136);
            this.txtServerPassword.Name = "txtServerPassword";
            this.txtServerPassword.Size = new System.Drawing.Size(185, 20);
            this.txtServerPassword.TabIndex = 11;
            // 
            // lblNumPlayers
            // 
            this.lblNumPlayers.AutoSize = true;
            this.lblNumPlayers.Location = new System.Drawing.Point(291, 247);
            this.lblNumPlayers.Name = "lblNumPlayers";
            this.lblNumPlayers.Size = new System.Drawing.Size(13, 13);
            this.lblNumPlayers.TabIndex = 9;
            this.lblNumPlayers.Text = "2";
            // 
            // lblPlayerCount
            // 
            this.lblPlayerCount.AutoSize = true;
            this.lblPlayerCount.Location = new System.Drawing.Point(25, 238);
            this.lblPlayerCount.Name = "lblPlayerCount";
            this.lblPlayerCount.Size = new System.Drawing.Size(67, 13);
            this.lblPlayerCount.TabIndex = 8;
            this.lblPlayerCount.Text = "Max Players:";
            this.lblPlayerCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trkNumPlayers
            // 
            this.trkNumPlayers.LargeChange = 1;
            this.trkNumPlayers.Location = new System.Drawing.Point(100, 238);
            this.trkNumPlayers.Maximum = 6;
            this.trkNumPlayers.Minimum = 2;
            this.trkNumPlayers.Name = "trkNumPlayers";
            this.trkNumPlayers.Size = new System.Drawing.Size(185, 45);
            this.trkNumPlayers.TabIndex = 7;
            this.trkNumPlayers.Value = 2;
            this.trkNumPlayers.ValueChanged += new System.EventHandler(this.NumPlayersTRackUpdated);
            // 
            // chkSimulateBotThink
            // 
            this.chkSimulateBotThink.AutoSize = true;
            this.chkSimulateBotThink.Location = new System.Drawing.Point(100, 199);
            this.chkSimulateBotThink.Name = "chkSimulateBotThink";
            this.chkSimulateBotThink.Size = new System.Drawing.Size(141, 17);
            this.chkSimulateBotThink.TabIndex = 6;
            this.chkSimulateBotThink.Text = "Simulate Bot Think Time";
            this.chkSimulateBotThink.UseVisualStyleBackColor = true;
            this.chkSimulateBotThink.CheckedChanged += new System.EventHandler(this.InputValueChanged);
            // 
            // lblBotDifficulty
            // 
            this.lblBotDifficulty.AutoSize = true;
            this.lblBotDifficulty.Location = new System.Drawing.Point(25, 167);
            this.lblBotDifficulty.Name = "lblBotDifficulty";
            this.lblBotDifficulty.Size = new System.Drawing.Size(69, 13);
            this.lblBotDifficulty.TabIndex = 5;
            this.lblBotDifficulty.Text = "Bot Difficulty:";
            this.lblBotDifficulty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trkBotDifficulty
            // 
            this.trkBotDifficulty.AutoSize = false;
            this.trkBotDifficulty.LargeChange = 10;
            this.trkBotDifficulty.Location = new System.Drawing.Point(100, 167);
            this.trkBotDifficulty.Maximum = 100;
            this.trkBotDifficulty.Name = "trkBotDifficulty";
            this.trkBotDifficulty.Size = new System.Drawing.Size(185, 26);
            this.trkBotDifficulty.TabIndex = 4;
            this.trkBotDifficulty.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkBotDifficulty.ValueChanged += new System.EventHandler(this.InputValueChanged);
            // 
            // txtServerDescription
            // 
            this.txtServerDescription.Location = new System.Drawing.Point(100, 45);
            this.txtServerDescription.Multiline = true;
            this.txtServerDescription.Name = "txtServerDescription";
            this.txtServerDescription.Size = new System.Drawing.Size(185, 79);
            this.txtServerDescription.TabIndex = 3;
            this.txtServerDescription.TextChanged += new System.EventHandler(this.InputValueChanged);
            // 
            // lblServerDescription
            // 
            this.lblServerDescription.AutoSize = true;
            this.lblServerDescription.Location = new System.Drawing.Point(6, 45);
            this.lblServerDescription.Name = "lblServerDescription";
            this.lblServerDescription.Size = new System.Drawing.Size(72, 13);
            this.lblServerDescription.TabIndex = 2;
            this.lblServerDescription.Text = "Server Desc.:";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(100, 19);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(185, 20);
            this.txtServerName.TabIndex = 1;
            this.txtServerName.TextChanged += new System.EventHandler(this.InputValueChanged);
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(6, 22);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(72, 13);
            this.lblServerName.TabIndex = 0;
            this.lblServerName.Text = "Server Name:";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 433);
            this.Controls.Add(this.grpHosting);
            this.Controls.Add(this.grpPlayerSettings);
            this.Controls.Add(this.btnSetDefaults);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.grpPlayerSettings.ResumeLayout(false);
            this.grpPlayerSettings.PerformLayout();
            this.grpHosting.ResumeLayout(false);
            this.grpHosting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkNumPlayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBotDifficulty)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnSetDefaults;
        private System.Windows.Forms.GroupBox grpPlayerSettings;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.GroupBox grpHosting;
        private System.Windows.Forms.CheckBox chkSimulateBotThink;
        private System.Windows.Forms.Label lblBotDifficulty;
        private System.Windows.Forms.TrackBar trkBotDifficulty;
        private System.Windows.Forms.TextBox txtServerDescription;
        private System.Windows.Forms.Label lblServerDescription;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Label lblNumPlayers;
        private System.Windows.Forms.Label lblPlayerCount;
        private System.Windows.Forms.TrackBar trkNumPlayers;
        private System.Windows.Forms.Label lblServerPassword;
        private System.Windows.Forms.TextBox txtServerPassword;
    }
}