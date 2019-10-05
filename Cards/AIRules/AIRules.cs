using DurakLibrary.Cards;
using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Collections.Generic;
using System.Linq;

namespace DurakGame.AIRules
{
    public class AIRules : IAIRule
    {
        public void Propose(Dictionary<Card, float> proposals, CoreDurakGame core, Player botplayer)
        {
            var state = core.GameState;
            var trumpSuit = state.GetValueCard(Names.TRUMP_CARD).Suit;
            Card[] keys = proposals.Keys.ToArray();

            if (botplayer.ID != core.GameState.GetValueInt(Names.DEFENDING_PLAYER))
            {
                foreach (var key in keys)
                {
                    if (key.Suit != trumpSuit)
                        proposals[key] += .25f;

                    if (state.GetValueBool(Names.IS_ATTACKING))
                        proposals[key] += .25f - (key.CardValue / 100.0f);

                    if (state.GetValueInt(Names.CURRENT_ROUND_THROWING) != 0)     
                        for (int i = 0; i >= state.GetValueInt(Names.CURRENT_ROUND_THROWING); i++)
                            if (state.GetValueCard(Names.ATTACKING_CARD, i).Value != key.Value 
                            ||  state.GetValueCard(Names.DEFENDING_CARD, i).Value != key.Value 
                            ||  key.Suit == trumpSuit)
                                proposals[key] = 0.0f;
                }
            }
            else
            {
                var attackingCard = state.GetValueCard(Names.ATTACKING_CARD, state.GetValueInt(Names.CURRENT_ROUND_DEFENDING));

                foreach (Card key in keys)
                    if (key.Suit == trumpSuit | key.Suit == attackingCard.Suit)
                    {
                        proposals[key] += .25f;
                        if (key.Suit != trumpSuit)
                            proposals[key] += .25f;
                        
                        if (key.Value > attackingCard.Value)
                            proposals[key] += .25f - (key.CardValue / 100.0f);
                        else if (key.Value < attackingCard.Value && key.Suit == attackingCard.Suit)
                            proposals[key] = 0.0f;
                    }
            }
        }
    }
}
