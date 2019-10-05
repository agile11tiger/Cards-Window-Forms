using System;
using System.Drawing;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            BackColor = pbxSplashScreen.BackColor;
            TransparencyKey = pbxSplashScreen.BackColor;

            var timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += TimerTicked;
            timer.Start();
        }
        
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            TimerTicked(this, e);
        }
        
        private void pbxSplashScreen_Click(object sender, EventArgs e)
        {
            TimerTicked(sender, e);
        }
        
        private void TimerTicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}
