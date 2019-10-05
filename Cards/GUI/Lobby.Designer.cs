namespace DurakGame
{
    partial class Lobby
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lobby));
            this.pnlPlayers = new System.Windows.Forms.Panel();
            this.pnlAddBot = new System.Windows.Forms.Panel();
            this.btnAddBot = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBotName = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.grpGameSettings = new System.Windows.Forms.GroupBox();
            this.chkSimulateBotThinkTime = new System.Windows.Forms.CheckBox();
            this.rbn52Cards = new System.Windows.Forms.RadioButton();
            this.rbn20Cards = new System.Windows.Forms.RadioButton();
            this.rbn36Cards = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.grpServerInfo = new System.Windows.Forms.GroupBox();
            this.lblServerDescription = new System.Windows.Forms.Label();
            this.lblServerName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.pnlPlayers.SuspendLayout();
            this.pnlAddBot.SuspendLayout();
            this.grpGameSettings.SuspendLayout();
            this.grpServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPlayers
            // 
            this.pnlPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlayers.Controls.Add(this.pnlAddBot);
            this.pnlPlayers.Location = new System.Drawing.Point(13, 14);
            this.pnlPlayers.Name = "pnlPlayers";
            this.pnlPlayers.Size = new System.Drawing.Size(270, 370);
            this.pnlPlayers.TabIndex = 0;
            // 
            // pnlAddBot
            // 
            this.pnlAddBot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAddBot.Controls.Add(this.btnAddBot);
            this.pnlAddBot.Controls.Add(this.label7);
            this.pnlAddBot.Controls.Add(this.txtBotName);
            this.pnlAddBot.Location = new System.Drawing.Point(5, 5);
            this.pnlAddBot.Name = "pnlAddBot";
            this.pnlAddBot.Size = new System.Drawing.Size(260, 60);
            this.pnlAddBot.TabIndex = 6;
            // 
            // btnAddBot
            // 
            this.btnAddBot.Location = new System.Drawing.Point(205, 2);
            this.btnAddBot.Name = "btnAddBot";
            this.btnAddBot.Size = new System.Drawing.Size(46, 46);
            this.btnAddBot.TabIndex = 8;
            this.btnAddBot.Text = "+";
            this.btnAddBot.UseVisualStyleBackColor = true;
            this.btnAddBot.Click += new System.EventHandler(this.btnAddBot_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Player Name:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBotName
            // 
            this.txtBotName.Location = new System.Drawing.Point(83, 17);
            this.txtBotName.Name = "txtBotName";
            this.txtBotName.Size = new System.Drawing.Size(102, 20);
            this.txtBotName.TabIndex = 2;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(12, 524);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Leave";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // grpGameSettings
            // 
            this.grpGameSettings.Controls.Add(this.chkSimulateBotThinkTime);
            this.grpGameSettings.Controls.Add(this.rbn52Cards);
            this.grpGameSettings.Controls.Add(this.rbn20Cards);
            this.grpGameSettings.Controls.Add(this.rbn36Cards);
            this.grpGameSettings.Controls.Add(this.label3);
            this.grpGameSettings.Location = new System.Drawing.Point(290, 151);
            this.grpGameSettings.Name = "grpGameSettings";
            this.grpGameSettings.Size = new System.Drawing.Size(190, 366);
            this.grpGameSettings.TabIndex = 3;
            this.grpGameSettings.TabStop = false;
            this.grpGameSettings.Text = "Game Settings";
            // 
            // chkSimulateBotThinkTime
            // 
            this.chkSimulateBotThinkTime.AutoSize = true;
            this.chkSimulateBotThinkTime.Location = new System.Drawing.Point(7, 20);
            this.chkSimulateBotThinkTime.Name = "chkSimulateBotThinkTime";
            this.chkSimulateBotThinkTime.Size = new System.Drawing.Size(141, 17);
            this.chkSimulateBotThinkTime.TabIndex = 6;
            this.chkSimulateBotThinkTime.Text = "Simulate Bot Think Time";
            this.chkSimulateBotThinkTime.UseVisualStyleBackColor = true;
            // 
            // rbn52Cards
            // 
            this.rbn52Cards.AutoSize = true;
            this.rbn52Cards.Location = new System.Drawing.Point(49, 104);
            this.rbn52Cards.Name = "rbn52Cards";
            this.rbn52Cards.Size = new System.Drawing.Size(67, 17);
            this.rbn52Cards.TabIndex = 4;
            this.rbn52Cards.Text = "52 Cards";
            this.rbn52Cards.UseVisualStyleBackColor = true;
            // 
            // rbn20Cards
            // 
            this.rbn20Cards.AutoSize = true;
            this.rbn20Cards.Location = new System.Drawing.Point(49, 58);
            this.rbn20Cards.Name = "rbn20Cards";
            this.rbn20Cards.Size = new System.Drawing.Size(67, 17);
            this.rbn20Cards.TabIndex = 3;
            this.rbn20Cards.Text = "20 Cards";
            this.rbn20Cards.UseVisualStyleBackColor = true;
            // 
            // rbn36Cards
            // 
            this.rbn36Cards.AutoSize = true;
            this.rbn36Cards.Checked = true;
            this.rbn36Cards.Location = new System.Drawing.Point(49, 81);
            this.rbn36Cards.Name = "rbn36Cards";
            this.rbn36Cards.Size = new System.Drawing.Size(67, 17);
            this.rbn36Cards.TabIndex = 2;
            this.rbn36Cards.TabStop = true;
            this.rbn36Cards.Text = "36 Cards";
            this.rbn36Cards.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Number of cards:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(405, 524);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(13, 497);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(192, 20);
            this.txtMessage.TabIndex = 5;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(209, 495);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // grpServerInfo
            // 
            this.grpServerInfo.Controls.Add(this.lblServerDescription);
            this.grpServerInfo.Controls.Add(this.lblServerName);
            this.grpServerInfo.Controls.Add(this.label5);
            this.grpServerInfo.Controls.Add(this.label4);
            this.grpServerInfo.Location = new System.Drawing.Point(290, 13);
            this.grpServerInfo.Name = "grpServerInfo";
            this.grpServerInfo.Size = new System.Drawing.Size(190, 132);
            this.grpServerInfo.TabIndex = 7;
            this.grpServerInfo.TabStop = false;
            this.grpServerInfo.Text = "Info";
            // 
            // lblServerDescription
            // 
            this.lblServerDescription.Location = new System.Drawing.Point(6, 49);
            this.lblServerDescription.Name = "lblServerDescription";
            this.lblServerDescription.Size = new System.Drawing.Size(183, 73);
            this.lblServerDescription.TabIndex = 3;
            this.lblServerDescription.Text = "DESCRIPTION";
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(84, 16);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(38, 13);
            this.lblServerName.TabIndex = 2;
            this.lblServerName.Text = "NAME";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Server Name:";
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(13, 390);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.Size = new System.Drawing.Size(271, 101);
            this.txtChat.TabIndex = 8;
            // 
            // Lobby
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 559);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.grpServerInfo);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpGameSettings);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.pnlPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Lobby";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lobby";
            this.pnlPlayers.ResumeLayout(false);
            this.pnlAddBot.ResumeLayout(false);
            this.pnlAddBot.PerformLayout();
            this.grpGameSettings.ResumeLayout(false);
            this.grpGameSettings.PerformLayout();
            this.grpServerInfo.ResumeLayout(false);
            this.grpServerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPlayers;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.GroupBox grpGameSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox grpServerInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlAddBot;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBotName;
        private System.Windows.Forms.RadioButton rbn52Cards;
        private System.Windows.Forms.RadioButton rbn20Cards;
        private System.Windows.Forms.RadioButton rbn36Cards;
        private System.Windows.Forms.Label lblServerDescription;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Button btnAddBot;
        private System.Windows.Forms.CheckBox chkSimulateBotThinkTime;
    }
}