using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Linq;

namespace DurakGame.Rules.MoveVerify
{
    public class VerifyTurnRule : IGamePlayRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Verify the player's turn"; }

        public bool IsValidMove(CoreDurakGame core, GameMove move, ref string reason)
        {
            if (move.Player.ID == core.GameState.GetValueInt(Names.ATTACKING_PLAYER))
                return true;
            else if (core.GameState.GetValueDictIntBool(Names.THROWING_PLAYERS).Keys.Contains(move.Player.ID) 
                 && !core.GameState.GetValueBool(Names.IS_ATTACKING))
                 return true;
            else if (move.Player.ID == core.GameState.GetValueInt(Names.DEFENDING_PLAYER))
                return true;

            reason = "It is not your turn to " + (core.GameState.GetValueBool(Names.IS_ATTACKING) ? "attack." : "defend.");
            return false;
        }
    }
}
