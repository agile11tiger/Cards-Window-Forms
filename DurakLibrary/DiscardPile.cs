using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DurakLibrary.Cards;

namespace DurakLibrary
{
    public partial class DiscardPile : UserControl
    {
        private struct CardInstance
        {
            public Card Card;
            public float Scale;
            public PointF Center;
            public float Rotation;
            
            public CardInstance(Card card, float rotation, PointF center)
            {
                Card = card;
                Rotation = rotation;
                Center = center;
                Scale = 1;
            }
        }
        
        private List<CardInstance> cardInstances;
        private Random random;
        private const float CARD_WIDTH = 48;
        private const float CARD_HEIGHT = 64;

        public DiscardPile()
        {
            InitializeComponent();
            cardInstances = new List<CardInstance>();
            random = new Random();
        }
        
        public void Clear()
        {
            cardInstances.Clear();
            Invalidate();
        }
        
        public void AddCard(Card card)
        {
            var offset = (int)((CARD_WIDTH + CARD_HEIGHT) / 2.0f);
            var point = new PointF(random.Next(offset, Width - offset), random.Next(offset, Height - offset));
            var instance = new CardInstance(card, random.Next(0, 360), point);
            cardInstances.Add(instance);
            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var instance in cardInstances)
            {
                e.Graphics.TranslateTransform(instance.Center.X, instance.Center.Y);
                e.Graphics.RotateTransform(instance.Rotation);
                e.Graphics.DrawImage(instance.Card.GetCardImage(instance.Card.ToString()), 0, 0, CARD_WIDTH, CARD_HEIGHT);
                e.Graphics.ResetTransform();
            }
        }
    }
}
