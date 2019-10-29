using DurakLibrary.Cards;
using DurakLibrary.Common;
using System.Collections.Generic;
using System.Linq;

namespace DurakGame.Rules
{
    static class Utils
    {
        public static void MoveNextDuel(CoreDurakGame core)
        {
            var state = core.GameState;
            var players = core.ConnectedServer.Players;

            if (!state.GetValueBool(Names.IS_GAME_OVER))
            {
                var forfeitingPlayers = core.GameState.GetValueDictIntBool(Names.THROWING_PLAYERS);
                var winningPlayers = core.GameState.GetValueListInt(Names.WINNING_PLAYERS);
                var deck = new Deck(state.GetValueCardCollection(Names.DECK));
                
                DealCards(state, players, winningPlayers, deck);

                var activePlayers = players.Values.Where(X => X.Hand.Count > 0).Count();
                var isDefenderForfeit = state.GetValueBool(Names.DEFENDER_FORFEIT);

                if (activePlayers > 1)
                {
                    state.Set(Names.IS_ATTACKING, true);
                    state.Set(Names.DEFENDER_FORFEIT, false);
                    state.Set(Names.DEFENDER_HAVE_NOT_CARDS, false);
                    state.Set(Names.DEFENDER_TIMER_FINISHED, false);
                    state.Set(Names.CURRENT_ROUND_THROWING, 0);
                    state.Set(Names.CURRENT_ROUND_DEFENDING, 0);
                    state.Set(Names.THROWING_PLAYERS, forfeitingPlayers);
                    state.Set(Names.WINNING_PLAYERS, winningPlayers);
                    state.Set(Names.THROWES_WITHOUT_CARDS, new List<int>());

                    var supportedPlayerCount = core.ConnectedServer.SupportedPlayerCount;
                    var tempDefendingID = state.GetValueInt(Names.DEFENDING_PLAYER);
                    var attackingID = FindIDAttackingOrDefending(players, supportedPlayerCount, isDefenderForfeit ? tempDefendingID + 1 : tempDefendingID);
                    var defendingID = FindIDAttackingOrDefending(players, supportedPlayerCount, attackingID + 1);

                    forfeitingPlayers.Clear();
                    players.Values.Select(p => p.ID)
                        .Except(state.GetValueListInt(Names.WINNING_PLAYERS))
                        .Where(i => i != defendingID)
                        .ToList()
                        .ForEach(i => forfeitingPlayers.Add(i, false));

                    state.Set(Names.DECK, deck.Cards);
                    state.Set(Names.ATTACKING_PLAYER, attackingID);
                    state.Set(Names.DEFENDING_PLAYER, defendingID);
                    state.Set(Names.AMOUNT_CARD_DEFENDER, core.ConnectedServer.Players[defendingID].Hand.Count);
                    state.Set(Names.CARDS_CAN_THROWN, core.ConnectedServer.Players[defendingID].Hand.Count);
                    state.Set(Names.DECK_COUNT, deck.Cards.Count);
                    state.Set(Names.THROWING_PLAYERS, forfeitingPlayers);
                }
                else
                {
                    if (activePlayers == 1)
                    {
                        state.Set(Names.LOSER_ID, players.Values.First(X => X.Hand.Count > 0).ID);
                        state.Set(Names.IS_TIE, false);
                        state.Set(Names.IS_GAME_OVER, true);
                    }
                    else if (activePlayers == 0)
                    {
                        state.Set(Names.IS_TIE, true);
                        state.Set(Names.IS_GAME_OVER, true);
                    }
                }
            }
        }

        public static int FindIDAttackingOrDefending(Dictionary<int, Player> players, int supportedPlayerCount, int id)
        {
            for (var iterations = 0; iterations < supportedPlayerCount; id++, iterations++)
            {
                id = id > supportedPlayerCount ? 0 : id;

                if (players.ContainsKey(id) && players[id].Hand.Count > 0)
                    return id;
            }

            return id;
        }

        public static int FindIDWithLeastTrump(Dictionary<int, Player> players, CardSuit trump)
        {
            var list = new List<Card>();

            foreach (var player in players.Values)
                foreach (var card in player.Hand)
                    list.Add(card);

            var leastTrumpCard = list
                .Where(c => c.Suit == trump)
                .OrderBy(c => c.Value)
                .FirstOrDefault();

            foreach (var player in players.Values)
            {
                var card = player.Hand.FirstOrDefault(c => c == leastTrumpCard);

                if (card != null)
                    return player.ID;
            }

            return players.Keys.OrderBy(id => id).Last();
        }

        private static void DealCards(GameState state, Dictionary<int, Player> players, List<int> winningPlayers, Deck deck)
        {
            var block = !state.GetValueBool(Names.TRUMP_CARD_USED);

            while (block && !players.Values.All(p => p.Hand.Count >= 6))
            {
                foreach (var player in players.Values)
                {
                    if (deck.Cards.Count > 0 && player.Hand.Count < 6)
                        player.Hand.Add(deck.Draw());
                    else if (block && deck.Cards.Count <= 0 && player.Hand.Count < 6)
                    {
                        player.Hand.Add(state.GetValueCard(Names.TRUMP_CARD));
                        state.Set(Names.TRUMP_CARD_USED, true);
                        block = false;
                        break;
                    }
                }
            }

            foreach (var player in players.Values)
                if (player.Hand.Count == 0 && !winningPlayers.Contains(player.ID))
                    winningPlayers.Add(player.ID);
        }
    }
}

