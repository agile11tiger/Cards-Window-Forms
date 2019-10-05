using DurakLibrary.Common;
using DurakLibrary.HostServer;

namespace DurakGame.AIRules
{
    public class VerifyBotState : IBotInvokeStateChecker
    {
        public bool ShouldInvoke(CoreDurakGame core, Player player)
        {
            var playerId = player.ID;

            if (core.GameState.GetValueBool(Names.IS_GAME_OVER) 
            &&  core.GameState.GetValueListInt(Names.WINNING_PLAYERS).Contains(playerId))
                return false;

            var defendingID = core.GameState.GetValueInt(Names.DEFENDING_PLAYER);
            var currentRoundDefending = core.GameState.GetValueInt(Names.CURRENT_ROUND_DEFENDING);
            var attackingCard = core.GameState.GetValueCard(Names.ATTACKING_CARD, currentRoundDefending);
            
            if (playerId != defendingID)
                return true;
            else if (attackingCard != null)
                return true;
            else
                return false;
        }
    }
}
