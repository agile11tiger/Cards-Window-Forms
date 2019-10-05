namespace DurakGame
{
    partial class MultiplayerMode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiplayerMode));
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnHost = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnJoin
            // 
            this.btnJoin.BackColor = System.Drawing.Color.DarkGreen;
            this.btnJoin.Font = new System.Drawing.Font("Georgia", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnJoin.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJoin.Location = new System.Drawing.Point(74, 120);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(202, 45);
            this.btnJoin.TabIndex = 3;
            this.btnJoin.Text = "JOIN";
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.JoinClick);
            this.btnJoin.MouseEnter += new System.EventHandler(this.ButtonMouseEntered);
            this.btnJoin.MouseLeave += new System.EventHandler(this.ButtonMouseLeft);
            // 
            // btnHost
            // 
            this.btnHost.BackColor = System.Drawing.Color.DarkGreen;
            this.btnHost.Font = new System.Drawing.Font("Georgia", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnHost.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnHost.Location = new System.Drawing.Point(74, 69);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(202, 45);
            this.btnHost.TabIndex = 4;
            this.btnHost.Text = "HOST";
            this.btnHost.UseVisualStyleBackColor = false;
            this.btnHost.Click += new System.EventHandler(this.HostClick);
            this.btnHost.MouseEnter += new System.EventHandler(this.ButtonMouseEntered);
            this.btnHost.MouseLeave += new System.EventHandler(this.ButtonMouseLeft);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.DarkGreen;
            this.btnBack.Font = new System.Drawing.Font("Georgia", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBack.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnBack.Location = new System.Drawing.Point(74, 171);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(202, 45);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "BACK";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BackClick);
            this.btnBack.MouseEnter += new System.EventHandler(this.ButtonMouseEntered);
            this.btnBack.MouseLeave += new System.EventHandler(this.ButtonMouseLeft);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.DarkGreen;
            this.lblTitle.Font = new System.Drawing.Font("Georgia", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(324, 56);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "Select Mode";
            // 
            // MultiplayerMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(352, 239);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnHost);
            this.Controls.Add(this.btnJoin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiplayerMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multiplayer Mode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnHost;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblTitle;
    }
}