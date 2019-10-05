namespace DurakGame
{
    partial class SplashScreen
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
            this.pbxSplashScreen = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSplashScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxSplashScreen
            // 
            this.pbxSplashScreen.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pbxSplashScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbxSplashScreen.Image = global::DurakGame.Properties.Resources.SplashScreen;
            this.pbxSplashScreen.Location = new System.Drawing.Point(37, 12);
            this.pbxSplashScreen.Name = "pbxSplashScreen";
            this.pbxSplashScreen.Size = new System.Drawing.Size(1102, 900);
            this.pbxSplashScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxSplashScreen.TabIndex = 0;
            this.pbxSplashScreen.TabStop = false;
            this.pbxSplashScreen.Click += new System.EventHandler(this.pbxSplashScreen_Click);
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(1200, 1000);
            this.Controls.Add(this.pbxSplashScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            ((System.ComponentModel.ISupportInitialize)(this.pbxSplashScreen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxSplashScreen;
    }
}