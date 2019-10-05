using System;

namespace DurakLibrary.Cards
{
    public class CardEventArgs : EventArgs
    {
        public Card Card { get; set; }
        public CardEventArgs(Card card)
        {
            Card = card;
        }
    }
}
