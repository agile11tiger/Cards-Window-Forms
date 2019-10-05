using DurakLibrary.Cards;
using DurakLibrary.Common;
using DurakLibrary.HostServer;

namespace DurakGame.Rules.MoveVerify
{
    class VerifyCardsOnTable : IGamePlayRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Verify cards on Table"; }

        public bool IsValidMove(CoreDurakGame core, GameMove move, ref string reason)
        {
            if (core.GameState.GetValueInt(Names.DEFENDING_PLAYER) == move.Player.ID)
                return true;

            var throwingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_THROWING);
            var defendingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_DEFENDING);

            if (throwingRound % 6 != 0)
                return true;
            else if (throwingRound - defendingRound == 0 || core.GameState.GetValueBool(Names.DEFENDER_FORFEIT))
            {
                var tempDiscard = core.GameState.GetValueCardCollection(Names.TEMP_DISCARD);

                for (var index = 0; index < throwingRound; index++)
                {
                    var attackingCard = core.GameState.GetValueCard(Names.ATTACKING_CARD, index);
                    if (attackingCard != null)
                        tempDiscard.Add(core.GameState.GetValueCard(Names.ATTACKING_CARD, index));

                    var defendingCard = core.GameState.GetValueCard(Names.DEFENDING_CARD, index);
                    if (defendingCard != null)
                        tempDiscard.Add(core.GameState.GetValueCard(Names.DEFENDING_CARD, index));

                    core.GameState.Set<Card>(Names.ATTACKING_CARD, index, null);
                    core.GameState.Set<Card>(Names.DEFENDING_CARD, index, null);
                }

                core.GameState.Set(Names.TEMP_DISCARD, tempDiscard);
                core.GameState.Set(Names.CURRENT_ROUND_THROWING, 0);
                core.GameState.Set(Names.CURRENT_ROUND_DEFENDING, 0);
                return true;
            }

            reason = "Wait until the player fight off the remaining cards and clears the table";
            return false;
        }
    }
}
