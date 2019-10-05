using DurakLibrary.Common;
using DurakLibrary.HostServer;

namespace DurakGame.Rules.MoveVerify
{
    public class VerifyCardInHand : IGamePlayRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Verify card in Hand"; }
        
        public bool IsValidMove(CoreDurakGame core, GameMove move, ref string reason)
        {
            if (core.GameState.GetValueListInt(Names.WINNING_PLAYERS).Contains(move.Player.ID))
            {
                reason = "You have already won!";
                return false;
            }
            else if (move.Move == null)
                return true;
            else if (core.GameState.GetValueListInt(Names.THROWES_WITHOUT_CARDS).Contains(move.Player.ID))
            {
                reason = "You haven`t cards, wait for the next round";
                return false;
            }
            else if (!move.Player.Hand.Contains(move.Move))
            {
                reason = "Card is not in players hand";
                return false;
            }
            else
                return true;
        }
    }
}
