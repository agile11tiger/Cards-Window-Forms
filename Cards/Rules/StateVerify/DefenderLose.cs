using DurakLibrary.Cards;
using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Linq;

namespace DurakGame.Rules.StateVerify
{
    public class DefenderLose : IGameStateRule
    {
        public bool IsEnabled { get; set; }
        public string ReadableName { get => "Check if the defending player has lost"; }
        
        public void ValidateState(CoreDurakGame core)
        {
            if (core.GameState.GetValueBool(Names.DEFENDER_FORFEIT) 
            && (core.GameState.GetValueBool(Names.DEFENDER_TIMER_FINISHED)
            ||  core.GameState.GetValueDictIntBool(Names.THROWING_PLAYERS).All(p => p.Value == true)))
            {
                var round = core.GameState.GetValueInt(Names.CURRENT_ROUND_THROWING);
                var defender = core.ConnectedServer.Players[core.GameState.GetValueInt(Names.DEFENDING_PLAYER)];
                var tempDiscard = core.GameState.GetValueCardCollection(Names.TEMP_DISCARD);

                for (int index = 0; index < round; index++)
                {
                    var attackingCard = core.GameState.GetValueCard(Names.ATTACKING_CARD, index);
                    if (attackingCard != null)
                        defender.Hand.Add(attackingCard);

                    var defendingCard = core.GameState.GetValueCard(Names.DEFENDING_CARD, index);
                    if (defendingCard != null)
                        defender.Hand.Add(defendingCard);
                    
                    core.GameState.Set<Card>(Names.ATTACKING_CARD, index, null);
                    core.GameState.Set<Card>(Names.DEFENDING_CARD, index, null);
                }

                foreach (var card in tempDiscard)
                    defender.Hand.Add(card);

                tempDiscard.Clear();
                core.GameState.Set(Names.TEMP_DISCARD, tempDiscard);
                Utils.MoveNextDuel(core);
            }
        }
    }
}
