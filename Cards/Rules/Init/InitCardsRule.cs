using DurakLibrary.Cards;
using DurakLibrary.Common;
using DurakLibrary.HostServer;

namespace DurakGame.Rules.Init
{
    class InitCardsRule : IGameInitRule
    {
        public bool IsEnabled { get; set; }
        public int Priority { get => 50; }
        public string ReadableName { get => "Deal cards"; }

        public void InitState(CoreDurakGame core)
        {
            Deck deck;
            var numInitCards = core.GameState.GetValueInt(Names.AMOUNT_INIT_CARDS);

            if (numInitCards == 20)
                deck = new Deck(CardValue.Ten, CardValue.Ace);
            else if (numInitCards == 52)
                deck = new Deck(CardValue.Two, CardValue.Ace);
            else
                deck = new Deck(CardValue.Six, CardValue.Ace);

            core.GameState.Set(Names.TRUMP_CARD, deck.Draw());

            foreach (var player in core.ConnectedServer.Players.Values)
            {
                for (var i = 0; i < 6; i++)
                {
                    if (deck.Cards.Count > 0)
                        player.Hand.Add(deck.Draw());
                    else if (!core.GameState.GetValueBool(Names.TRUMP_CARD_USED))
                    {
                        player.Hand.Add(core.GameState.GetValueCard(Names.TRUMP_CARD));
                        core.GameState.Set(Names.TRUMP_CARD_USED, true);
                    }
                }
            }  

            core.GameState.Set(Names.DECK, deck.Cards);
            core.GameState.Set(Names.DECK_COUNT, deck.Cards.Count);
        }
    }
}
