namespace DurakGame
{
    partial class PlayerView
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
            this.components = new System.ComponentModel.Container();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.imgPlayerType = new System.Windows.Forms.PictureBox();
            this.imgReady = new System.Windows.Forms.PictureBox();
            this.cmsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.imgPlayerType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgReady)).BeginInit();
            this.cmsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Location = new System.Drawing.Point(59, 21);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(73, 13);
            this.lblPlayerName.TabIndex = 0;
            this.lblPlayerName.Text = "[Player Name]";
            // 
            // imgPlayerType
            // 
            this.imgPlayerType.Location = new System.Drawing.Point(5, 5);
            this.imgPlayerType.Margin = new System.Windows.Forms.Padding(0);
            this.imgPlayerType.Name = "imgPlayerType";
            this.imgPlayerType.Size = new System.Drawing.Size(50, 50);
            this.imgPlayerType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgPlayerType.TabIndex = 2;
            this.imgPlayerType.TabStop = false;
            // 
            // imgReady
            // 
            this.imgReady.Location = new System.Drawing.Point(205, 5);
            this.imgReady.Margin = new System.Windows.Forms.Padding(0);
            this.imgReady.Name = "imgReady";
            this.imgReady.Size = new System.Drawing.Size(50, 50);
            this.imgReady.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgReady.TabIndex = 1;
            this.imgReady.TabStop = false;
            // 
            // cmsContextMenu
            // 
            this.cmsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kickToolStripMenuItem});
            this.cmsContextMenu.Name = "cmsContextMenu";
            this.cmsContextMenu.Size = new System.Drawing.Size(97, 26);
            // 
            // kickToolStripMenuItem
            // 
            this.kickToolStripMenuItem.Name = "kickToolStripMenuItem";
            this.kickToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.kickToolStripMenuItem.Text = "Kick";
            this.kickToolStripMenuItem.Click += new System.EventHandler(this.KickPlayerPressed);
            // 
            // PlayerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.imgPlayerType);
            this.Controls.Add(this.imgReady);
            this.Controls.Add(this.lblPlayerName);
            this.Name = "PlayerView";
            this.Size = new System.Drawing.Size(260, 60);
            ((System.ComponentModel.ISupportInitialize)(this.imgPlayerType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgReady)).EndInit();
            this.cmsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.PictureBox imgReady;
        private System.Windows.Forms.PictureBox imgPlayerType;
        private System.Windows.Forms.ContextMenuStrip cmsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem kickToolStripMenuItem;
    }
}
