using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DurakGame.Rules
{
    public class CardPlayed : IMoveSucessRule
    {
        public void UpdateState(CoreDurakGame core, GameMove move)
        {
            if (move.Move != null)
                move.Player.Hand.Remove(move.Move);
            else
                core.GameState.Set(Names.PLAYER_FORFEIT, move.Player.ID);

            var throwingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_THROWING);
            var defendingID = core.GameState.GetValueInt(Names.DEFENDING_PLAYER);
            var forfeitingPlayers = core.GameState.GetValueDictIntBool(Names.THROWING_PLAYERS);
            var throwesWithoutCards = core.GameState.GetValueListInt(Names.THROWES_WITHOUT_CARDS);
            var cardsCanThrown = core.GameState.GetValueInt(Names.CARDS_CAN_THROWN);

            if (core.GameState.GetValueBool(Names.IS_ATTACKING))
            {
                core.GameState.Set(Names.IS_ATTACKING, false);
                core.GameState.Set(Names.ATTACKING_CARD, throwingRound, move.Move);
                core.GameState.Set(Names.CURRENT_ROUND_THROWING, ++throwingRound);
                core.GameState.Set(Names.CARDS_CAN_THROWN, --cardsCanThrown);
            }
            else if (move.Player.ID != defendingID)
            {
                if (move.Move == null)
                    forfeitingPlayers[move.Player.ID] = true;
                else if (move.Move != null && !forfeitingPlayers[move.Player.ID])
                {
                    forfeitingPlayers.Keys.ToList().ForEach(v => forfeitingPlayers[v] = false);
                    core.GameState.Set(Names.ATTACKING_CARD, throwingRound, move.Move);
                    core.GameState.Set(Names.CURRENT_ROUND_THROWING, ++throwingRound);
                    core.GameState.Set(Names.CARDS_CAN_THROWN, --cardsCanThrown);
                }

                core.GameState.Set(Names.THROWING_PLAYERS, forfeitingPlayers);
            }

            if (move.Player.ID == defendingID)
            {
                var defenderForfeit = core.GameState.GetValueBool(Names.DEFENDER_FORFEIT);
                var defendingRound = core.GameState.GetValueInt(Names.CURRENT_ROUND_DEFENDING);

                if (move.Player.Hand.Count == 0)
                    core.GameState.Set(Names.DEFENDER_HAVE_NOT_CARDS, true);
                else if (move.Move == null && !defenderForfeit)
                {
                    core.GameState.Set(Names.DEFENDER_FORFEIT, true);
                    CardTakingCounter(core);
                    return;
                }
                else if (move.Move != null && !defenderForfeit)
                {
                    if (throwesWithoutCards != null && throwesWithoutCards.Count != 0)
                        forfeitingPlayers.Keys.Except(throwesWithoutCards).ToList().ForEach(v => forfeitingPlayers[v] = false);
                    else
                        forfeitingPlayers.Keys.ToList().ForEach(v => forfeitingPlayers[v] = false);

                    core.GameState.Set(Names.THROWING_PLAYERS, forfeitingPlayers);
                    core.GameState.Set(Names.REMOVE_ALL_FORFEITS, true);
                    core.GameState.Set(Names.DEFENDING_CARD, defendingRound, move.Move);
                    core.GameState.Set(Names.CURRENT_ROUND_DEFENDING, ++defendingRound);
                    core.GameState.Set(Names.AMOUNT_CARD_DEFENDER, move.Player.Hand.Count);
                }
            }
            
            if (move.Player.Hand.Count == 0 && move.Player.ID != defendingID)
            {
                throwesWithoutCards.Add(move.Player.ID);
                core.GameState.Set(Names.THROWES_WITHOUT_CARDS, throwesWithoutCards);

                forfeitingPlayers[move.Player.ID] = true;
                core.GameState.Set(Names.THROWING_PLAYERS, forfeitingPlayers);
            }
        }

        private async void CardTakingCounter(CoreDurakGame core)
        {
            TaskScheduler sync;

            if (core.IsSinglePlayerMode)
                sync = TaskScheduler.FromCurrentSynchronizationContext();
            else
                sync = TaskScheduler.Default;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    if (!core.GameState.GetValueBool(Names.DEFENDER_FORFEIT)) return false;
                    Thread.Sleep(100);
                }
                return true;
            })
            .ContinueWith(antecedent =>
            {
                if (antecedent.Result)
                {
                    PlayerServer.Mutex?.WaitOne();
                    core.GameState.Set(Names.DEFENDER_TIMER_FINISHED, true);
                    core.CheckStateRules();
                    PlayerServer.Mutex?.ReleaseMutex();
                }
            }, sync);
        }
    }
}
