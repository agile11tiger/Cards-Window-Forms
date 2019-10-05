namespace DurakLibrary.Cards
{
    public static class CardsComparerHelper
    {
        internal static CardSuit TrumpSuit { get; set; }

        public static bool IsTrump(Card card)
        {
            return TrumpSuit == card.Suit;
        }

        public static int Compare(Card up, Card down)
        {
            if (up is null && down is null) return 0;
            if (up is null) return -1;
            if (down is null) return 1;

            if (up.Suit == down.Suit)
            {
                if (up.Value == down.Value) return 0;
                if (up.Value > down.Value) return 1;
                return -1;
            }
            else if (IsTrump(up)) return 1;
            else if (IsTrump(down)) return -1;
            else return -1; 
        }
    }
}
