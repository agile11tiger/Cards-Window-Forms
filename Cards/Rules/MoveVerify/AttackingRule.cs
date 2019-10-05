using DurakLibrary.Common;
using DurakLibrary.HostServer;

namespace DurakGame.Rules.MoveVerify
{
    public class AttackingRule : IGamePlayRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Rule for Attacker"; }
        
        public bool IsValidMove(CoreDurakGame core, GameMove move, ref string reason)
        {
            if (core.GameState.GetValueBool(Names.IS_ATTACKING))
            {
                if (move.Move == null && move.Player.ID == core.GameState.GetValueInt(Names.ATTACKING_PLAYER))
                {
                    reason = "Attacker cannot forfeit!";
                    return false;
                }
                if (move.Move == null)
                {
                    reason = "Now is the attacker's turn!";
                    return false;
                }
                else return true;
            }
            else 
                return true;
        }
    }
}
