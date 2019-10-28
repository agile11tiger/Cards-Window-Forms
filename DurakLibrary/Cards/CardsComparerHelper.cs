namespace DurakLibrary.Cards
{
    public static class CardsComparerHelper
    {
        //return: -1(up < down); 0(up == down); 1(up > down); 
        //or default -9, when cards have different suits (except comparison trumpsCard and notTrumpsCard).
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
            else if (Deck.IsTrump(up)) return 1;
            else if (Deck.IsTrump(down)) return -1;
            else return -9; 
        }
    }
}
