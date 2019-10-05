using DurakLibrary.Common;
using DurakLibrary.HostServer;

namespace DurakGame.Rules.MoveVerify
{
    public class DefendingRule : IGamePlayRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Defending Rule"; }
        
        public bool IsValidMove(CoreDurakGame core, GameMove move, ref string reason)
        {
            if (move.Move == null)
                return true;
            
            if (core.GameState.GetValueInt(Names.DEFENDING_PLAYER) == move.Player.ID)
            {
                var throwingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_THROWING);
                var defendingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_DEFENDING);
                var attackingCard = core.GameState.GetValueCard(Names.ATTACKING_CARD, defendingRound);
                var trumpSuit = core.GameState.GetValueCard(Names.TRUMP_CARD).Suit;

                if (throwingRound - defendingRound <= 0)
                {
                    reason = "This is not your turn";
                    return false;
                }
                else if (core.GameState.GetValueBool(Names.DEFENDER_FORFEIT))
                {
                    reason = "You already clicked FORFEIT";
                    return false;
                }
                else if (move.Move.Value > attackingCard.Value)
                {
                    if (move.Move.Suit == attackingCard.Suit || move.Move.Suit == trumpSuit)
                        return true;
                    else
                    {
                        reason = "You must play a card of a higher rank of the same suit, or a trump card";
                        return false;
                    }
                }
                else if (move.Move.Suit == trumpSuit && attackingCard.Suit != trumpSuit)
                        return true;
                else
                {
                    reason = "You must play a card of a higher rank";
                    return false;
                }
            }
            else
                return true;
        }
    }
}
