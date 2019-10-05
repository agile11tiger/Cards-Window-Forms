using System;
using System.Linq;

namespace DurakLibrary.Cards
{
    public delegate void LastCardDrawnHandler(Deck currentDeck);

    public class Deck
    {
        public event LastCardDrawnHandler LastCardDrawn;
        public readonly CardSuit TrumpSuit;
        public readonly CardCollection Cards = new CardCollection();
        
        public Card Draw()
        {
            if (Cards.Count == 0) return null;
            else
            {
                var card = Cards.Last();
                card.FaceUp = true;
                Cards.Remove(card);

                if (Cards.Count == 0 && LastCardDrawn != null)
                    LastCardDrawn(this);
                
                return card;
            }
        }

        public Deck(CardCollection newCards)
        {
            Cards = newCards;
        }

        public Deck(CardValue minValue, CardValue maxValue)
        {
            for (var suit = 0; suit < 4; suit++)
                for (var value = (int)minValue; value < (int)maxValue + 1; value++)
                    Cards.Add(new Card((CardValue)value, (CardSuit)suit));

            Shuffle();
            TrumpSuit = Cards.Last().Suit;
            CardsComparerHelper.TrumpSuit = TrumpSuit;
        }
        
        public void Shuffle()
        {
            var shuffledcards = Cards.OrderBy(card => Guid.NewGuid()).ToList();
            Cards.Clear();

            foreach (var card in shuffledcards)
                Cards.Add(card);
        }
    }
}
