using DurakLibrary.Cards;
using DurakLibrary.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace DurakLibrary.HostServer
{
    public class BotPlayer
    {
        private Random random;
        private float difficulty;
        public static bool SimulateThinkTime;
        public float Difficulty { get => difficulty; set => difficulty = value < 0 ? 0 : value > 1 ? 1 : value; }

        private CoreDurakGame core;
        public Player Player { get ; private set; }
        public bool ShouldInvoke { get; set; }
        public Dictionary<Card, float> ProposedMoves { get; private set; }

        public BotPlayer()
        {
        }

        public void SetBotPlayer(CoreDurakGame core, Player player, float difficulty)
        {
            this.core = core;
            Player = player;
            Difficulty = difficulty;

            random = new Random(player.ID + DateTime.Now.Millisecond);

            ProposedMoves = new Dictionary<Card, float>();
            player.Hand.CollectionChanged += CardAddedOrRemoved;
        }
        
        private void CardAddedOrRemoved(object sender, NotifyCollectionChangedEventArgs e)
        {
            var list = e.NewItems ?? e.OldItems;
            var card = list?[0] as Card;

            if (card != null && !ProposedMoves.ContainsKey(card))
            {
                card.FaceUp = true;
                ProposedMoves.Add(card, 0.0f);
                return;
            }
        }
        
        public bool ShouldMove()
        {
            foreach (IBotInvokeStateChecker stateChecker in Rules.BotInvokeRules)
                if (!stateChecker.ShouldInvoke(core, Player))
                    return false;

            return true;
        }

        public Card DetermineMove()
        {
            ProposedMoves.Keys.ToList().ForEach(card => ProposedMoves[card] = 0);
            Rules.AIRules.ForEach(rule => rule.Propose(ProposedMoves, core, Player));

            var sortedList = ProposedMoves.OrderByDescending(c => c.Value).ToList();
            var noMoveConfidence = 0f;
            ShouldInvoke = false;

            for (var index = 0; index < sortedList.Count; index++)
                if (sortedList[index].Value > noMoveConfidence && core.CanPlayMove(Player, sortedList[index].Key))
                    return sortedList[index].Key;

            return null;
        }
    }
}
