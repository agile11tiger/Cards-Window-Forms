using System;
using System.Drawing;
using System.Windows.Forms;
using DurakLibrary.Cards;

namespace DurakLibrary
{
    public partial class CardBox : UserControl
    {
        private Card card;
        private Image image;
        private Orientation orientation;

        public Card Card { get => card;
            set
            {
                card = value;

                if (card != null)
                    image = card.GetCardImage(card.ToString());

                Invalidate();
            }
        }
        
        public CardSuit Suit { get => Card.Suit;
            set
            {
                Card.Suit = value;
                Invalidate();
            }
        }
        
        public CardValue Value { get => Card.Value;
            set
            {
                Card.Value = value;
                Invalidate();
            }
        }
        
        public bool FaceUp { get => Card.FaceUp;
            set
            {
                if (card.FaceUp != value) 
                {
                    card.FaceUp = value; 
                    Invalidate();
                }
            }
        }
        
        public Orientation CardOrientation { get => orientation;
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    Size = new Size(Size.Height, Size.Width);
                    Invalidate();
                }
            }
        }
        
        protected override CreateParams CreateParams
        {
            get
            {
                var parameters = base.CreateParams;
                parameters.ExStyle |= 0x20; 
                return parameters;
            }
        }
        
        public CardBox()
        {
            InitializeComponent();
            orientation = Orientation.Vertical; 
            card = new Card();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }
        
        public CardBox(Card card, Orientation orientation = Orientation.Vertical) : this()
        {
            this.orientation = orientation; 
            this.card = card; 
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var image = card?.GetCardImage(card?.ToString());

            if (image != null)
            {
                var disabledColor = Color.FromArgb(Enabled ? 0 : 128, 128, 128, 128);

                using (var brush = new SolidBrush(disabledColor))
                {
                    e.Graphics.DrawImage(image, 0, 0, Width, Height);
                    e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
                }
            }
        }
    }
}
