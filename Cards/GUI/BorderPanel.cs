using System;
using System.Drawing;
using System.Windows.Forms;

namespace DurakGame
{
    public class BorderPanel : Panel
    {
        private float borderWidth;
        private Color borderColor;
        private bool showBorder;

        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Border width cannot be less than 0");

                borderWidth = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        public bool ShowBorder
        {
            get => showBorder;
            set
            {
                showBorder = value;
                Invalidate();
            }
        }

        public BorderPanel()
        {
            borderColor = Color.Black;
            BorderWidth = 2;
            ShowBorder = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (ShowBorder)
            {
                var pen = new Pen(BorderColor, BorderWidth);
                e.Graphics.DrawRectangle(pen, BorderWidth / 2, BorderWidth / 2, Width - BorderWidth, Height - BorderWidth);
            }
        }
    }
}
