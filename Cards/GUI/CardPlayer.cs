using System;
using System.Drawing;
using System.Windows.Forms;
using DurakLibrary.Cards;
using System.Collections.Specialized;
using DurakLibrary.Common;

namespace DurakGame
{
    public partial class CardPlayer : UserControl
    {
        public CardPlayer()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public event EventHandler<CardEventArgs> OnCardSelected;

        public Player Player
        {
            get => player;
            set
            {
                if (player != null)
                    player.Hand.CollectionChanged -= OnCardsChanged;

                player = value;

                if (player != null)
                    player.Hand.CollectionChanged += OnCardsChanged;
            }
        }

        public void UpdatePlayer()
        {
            Invalidate();
            CalculateSize();
        }

        protected override void OnResize(EventArgs e)
        {
            CalculateSize();
            base.OnResize(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hoverIndex = -1;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            hoverIndex = (int)((e.X - cardOffsetX) / cardWidth);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (player.Hand != null && hoverIndex > -1 && hoverIndex < player.Hand.Count)
                OnCardSelected?.Invoke(this, new CardEventArgs(player.Hand[hoverIndex]));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (player?.Hand != null && player.Hand.Count > 0)
            {
                for (var index = 0; index < player.Hand.Count; index++)
                    if (index != hoverIndex)
                    {
                        var image = player.Hand[index].GetCardImage(player.Hand[index].ToString());
                        e.Graphics.DrawImage(image, CalculateCardBounds(index));
                    }

                if (hoverIndex > -1 && hoverIndex < player.Hand.Count)
                {
                    var image = player.Hand[hoverIndex].GetCardImage(player.Hand[hoverIndex].ToString());
                    var bounds = CalculateCardBounds(hoverIndex);

                    bounds.Location = new PointF(bounds.Left - EXPAND_SIZE / 2, bounds.Top - EXPAND_SIZE / 2);
                    bounds.Width += EXPAND_SIZE;
                    bounds.Height += EXPAND_SIZE;
                    e.Graphics.DrawImage(image, bounds);
                    var pen = Pens.Black;
                    e.Graphics.DrawRoundedRectangle(pen, bounds, 4.0f);
                }
            }
            base.OnPaint(e);
        }

        private const float CARD_ASPECT_RATIO = 1.466666666666f;
        private const float EXPAND_SIZE = 16.0f;
        private float cardWidth;
        private float cardHeight;
        private float cardOffsetX;
        private float cardOffsetY;
        private int hoverIndex = -1;
        private Player player;

        private void OnCardsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePlayer();
        }

        private void CalculateSize()
        {
            if (player != null && player.Hand.Count > 0)
            {
                float availableWidth = Width - EXPAND_SIZE;
                float availableHeight = Height - EXPAND_SIZE;
                cardWidth = availableWidth / player.Hand.Count;
                cardHeight = cardWidth * CARD_ASPECT_RATIO;

                if (cardHeight > availableHeight)
                {
                    cardHeight = availableHeight;
                    cardWidth = cardHeight * (1 / CARD_ASPECT_RATIO);
                }

                cardOffsetX = EXPAND_SIZE / 2 + (availableWidth - (cardWidth * player.Hand.Count)) / 2.0f;
                cardOffsetY = EXPAND_SIZE / 2 + (availableHeight - cardHeight) / 2.0f;

            }
            else
                cardHeight = cardWidth = cardOffsetX = cardOffsetY = 0;
        }

        private RectangleF CalculateCardBounds(int index)
        {
            return new RectangleF(
                            cardOffsetX + cardWidth * index,
                            cardOffsetY,
                            cardWidth,
                            cardHeight);
        }
    }
}
