using DurakLibrary.Cards;
using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Linq;

namespace DurakGame.Rules.StateVerify
{
    public class AttackerLose : IGameStateRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Check if the attacking player has lost"; }

        public void ValidateState(CoreDurakGame core)
        {
            var throwingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_THROWING);
            var defendingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_DEFENDING);

            if (!core.GameState.GetValueBool(Names.DEFENDER_FORFEIT) && throwingRound - defendingRound == 0
            && core.GameState.GetValueDictIntBool(Names.THROWING_PLAYERS).All(p => p.Value == true))
            {
                var discard = core.GameState.GetValueCardCollection(Names.DISCARD);
                var tempDiscard = core.GameState.GetValueCardCollection(Names.TEMP_DISCARD);

                for (int index = 0; index < throwingRound; index++)
                {
                    if (core.GameState.GetValueCard(Names.ATTACKING_CARD, index) != null)
                        discard.Add(core.GameState.GetValueCard(Names.ATTACKING_CARD, index));
                    
                    if (core.GameState.GetValueCard(Names.DEFENDING_CARD, index) != null)
                        discard.Add(core.GameState.GetValueCard(Names.DEFENDING_CARD, index));

                    core.GameState.Set<Card>(Names.ATTACKING_CARD, index, null);
                    core.GameState.Set<Card>(Names.DEFENDING_CARD, index, null);
                }

                discard.Concat(tempDiscard);
                core.GameState.Set(Names.TEMP_DISCARD, new CardCollection());
                core.GameState.Set(Names.DISCARD, discard);
                Utils.MoveNextDuel(core);
            }
        }
    }
}
