using DurakLibrary.Cards;
using DurakLibrary.Clients;
using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Collections.Generic;
using System.Linq;

namespace DurakGame.Rules.Init
{
    public class InitRoundStates : IGameInitRule
    {
        public bool IsEnabled { get; set; }
        public int Priority { get => 100; }
        public string ReadableName { get => "Initialize start game states"; }

        public void InitState(CoreDurakGame core)
        {
            var players = core.ConnectedServer.Players;
            var attackingID = Utils.FindIDWithLeastTrump(players, core.GameState.GetValueCard(Names.TRUMP_CARD).Suit);
            var defendingID = Utils.FindIDAttackingOrDefending(players, core.ConnectedServer.SupportedPlayerCount, attackingID + 1);
            var forfeitingPlayers = new Dictionary<int, bool>();

            core.ConnectedServer.Players.Values
                .Select(p => p.ID)
                .Where(i => i != defendingID)
                .ToList()
                .ForEach(i => forfeitingPlayers.Add(i, false));

            core.GameState.Set(Names.IS_ATTACKING, true);
            core.GameState.Set(Names.DEFENDER_FORFEIT, false);
            core.GameState.Set(Names.AMOUNT_CARD_DEFENDER, 6);
            core.GameState.Set(Names.CARDS_CAN_THROWN, 6);
            core.GameState.Set(Names.DEFENDER_TIMER_FINISHED, false);
            core.GameState.Set(Names.ATTACKING_PLAYER, attackingID);
            core.GameState.Set(Names.DEFENDING_PLAYER, defendingID);
            core.GameState.Set(Names.THROWING_PLAYERS, forfeitingPlayers);
            core.GameState.Set(Names.THROWES_WITHOUT_CARDS, new List<int>());
            core.GameState.Set(Names.WINNING_PLAYERS, new List<int>());
            core.GameState.Set(Names.DISCARD, new CardCollection());
            core.GameState.Set(Names.TEMP_DISCARD, new CardCollection());

            for (int index = 0; index < 6; index++)
            {
                core.GameState.Set<Card>(Names.DEFENDING_CARD, index, null);
                core.GameState.Set<Card>(Names.ATTACKING_CARD, index, null);
            }
        }
    }
}
