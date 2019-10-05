using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Linq;

namespace DurakGame.Rules.MoveVerify
{
    class ThowingRule : IGamePlayRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Rule for Thrower"; }

        public bool IsValidMove(CoreDurakGame core, GameMove move, ref string reason)
        {
            var throwingPlayers = core.GameState.GetValueDictIntBool(Names.THROWING_PLAYERS);

            if (throwingPlayers.Keys.Contains(move.Player.ID) 
            && !core.GameState.GetValueBool(Names.IS_ATTACKING))
            {
                if (throwingPlayers[move.Player.ID])
                {
                    reason = "You already clicked FORFEIT";
                    return false;
                }

                if (move.Move == null)
                    return true;
                
                var throwingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_THROWING);
                var cardsCanThrown = core.GameState.GetValueInt(Names.CARDS_CAN_THROWN);
                var tempDiscard = core.GameState.GetValueCardCollection(Names.TEMP_DISCARD);

                if (cardsCanThrown <= 0)
                {
                    reason = "The player does not have enough cards to beat off more";
                    return false;
                }
                
                for (int index = 0; index < throwingRound; index++)
                    if (move.Move.Value == core.GameState.GetValueCard(Names.ATTACKING_CARD, index)?.Value 
                    ||  move.Move.Value == core.GameState.GetValueCard(Names.DEFENDING_CARD, index)?.Value)
                        return true;

                foreach (var card in tempDiscard)
                    if (move.Move.Value == card.Value)
                        return true;
                        
                reason = "You must play a card with a rank that has already been played";
                return false;
            }
            else
                return true;
        }
    }
}
