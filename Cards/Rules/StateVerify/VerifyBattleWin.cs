using DurakLibrary.Cards;
using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Linq;

namespace DurakGame.Rules.StateVerify
{
    public class VerifyBattleWin : IGameStateRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Check won battle"; }

        public void ValidateState(CoreDurakGame core)
        {
            if (core.GameState.GetValueBool(Names.DEFENDER_HAVE_NOT_CARDS))
            {
                var round = core.GameState.GetValueInt(Names.CURRENT_ROUND_THROWING);
                var discard = core.GameState.GetValueCardCollection(Names.DISCARD);
                var tempDiscard = core.GameState.GetValueCardCollection(Names.TEMP_DISCARD);

                for (int index = 0; index < round; index++)
                {
                    var attackingCard = core.GameState.GetValueCard(Names.ATTACKING_CARD, index);
                    if (attackingCard != null)
                        discard.Add(core.GameState.GetValueCard(Names.ATTACKING_CARD, index));

                    var defendingCard = core.GameState.GetValueCard(Names.DEFENDING_CARD, index);
                    if (defendingCard != null)
                        discard.Add(core.GameState.GetValueCard(Names.DEFENDING_CARD, index));

                    core.GameState.Set<Card>(Names.ATTACKING_CARD, index, null);
                    core.GameState.Set<Card>(Names.DEFENDING_CARD, index, null);
                }

                if (tempDiscard != null)
                    discard.Concat(tempDiscard);

                tempDiscard.Clear();
                core.GameState.Set(Names.TEMP_DISCARD, tempDiscard);
                core.GameState.Set(Names.DISCARD, discard);
                Utils.MoveNextDuel(core);
            }
        }
    }
}
