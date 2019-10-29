using System;
using System.Drawing;

namespace DurakLibrary.Cards
{
    public class Card : ICloneable
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }
        public int CardValue { get; set; }
        public bool FaceUp { get; set; }

        public Card() : this(Cards.CardValue.Ace, CardSuit.Hearts)
        {

        }

        public Card(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
            CardValue = (int)Value;
        }
        
        public Image GetCardImage(string imageName) 
        {
            var cardImage = Properties.Resources.ResourceManager.GetObject(imageName) as Image;
            return cardImage;
        }
        
        public override string ToString()
        {
            if (FaceUp)
            {
                if (Value == Cards.CardValue.Joker)
                {
                    if (Suit == CardSuit.Clubs || Suit == CardSuit.Spades)
                        return $"{Value}_Black";
                    else 
                        return $"{Value}_Red"; ;
                }
                else 
                    return $"{Suit}_{Value}";
            }
            else 
                return "Back";
        }
        
        public static bool operator ==(Card up, Card down)
        {
            switch (CardsComparerHelper.Compare(up, down))
            {
                case 0:
                    return true;
                case -1:
                case 1:
                default:
                    return false;
            }
        }

        public static bool operator !=(Card up, Card down)
        {
            switch (CardsComparerHelper.Compare(up, down))
            {
                case 0:
                    return false;
                case -1:
                case 1:
                default:
                    return true;
            }
        }

        public static bool operator >(Card up, Card down)
        {
            switch (CardsComparerHelper.Compare(up, down))
            {
                case 1:
                    return true;
                case 0:
                case -1:
                default:
                    return false;
            }
        }

        public static bool operator <(Card up, Card down)
        {
            switch (CardsComparerHelper.Compare(up, down))
            {
                case -1:
                    return true;
                case 0:
                case 1:
                default:
                    return false;
            }
        }

        public static bool operator >=(Card up, Card down)
        {
            switch (CardsComparerHelper.Compare(up, down))
            {
                case 0:
                case 1:
                    return true;
                case -1:
                default:
                    return false;
            }
        }

        public static bool operator <=(Card up, Card down)
        {
            switch (CardsComparerHelper.Compare(up, down))
            {
                case 0:
                case -1:
                    return true;
                case 1:
                default:
                    return false;
            }
        }

        public override bool Equals(object obj) => obj is Card && Value == (obj as Card).Value && Suit == (obj as Card).Suit;
        public override int GetHashCode() => (int)Value * 397 + (int)Suit * 397 + (FaceUp ? 1 : 0);
        public object Clone() => MemberwiseClone();
    } 
}
