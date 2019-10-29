using DurakLibrary.Cards;
using DurakLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DurakLibrary.HostServer
{
    public class SingleClientServer : CoreDurakGame
    {
        public event Action<Player> OnBotAdded;
        public event Action<int> OnBotRemoved;
        public event Action<Player, int, bool> OnAddedCard;
        public event Action<Player, int, bool> OnRemovedCard;

        public SingleClientServer(Player player) : base(player)
        {
        }

        public void CreatePlayersID()
        {
            var shuffleNumbers = Enumerable.Range(1, 5).OrderBy(n => Guid.NewGuid()).ToList();
            playersID = new Queue<int>(shuffleNumbers);
        }
        
        public void AddBot(float difficulty, string botName)
        {
            if (IsSinglePlayerMode)
            {
                if (string.IsNullOrWhiteSpace(botName))
                {
                    var rand = new Random();
                    var randomLineNumber = rand.Next(0, BotNames.Length - 1);
                    botName = BotNames[randomLineNumber].Trim();
                    botName = char.ToUpper(botName[0]) + botName.Substring(1);

                    if (botName.Length > 7)
                        botName = botName.Remove(7);
                }

                if (ConnectedServer.PlayersCount <= ConnectedServer.SupportedPlayerCount)
                {
                    var newPlayer = new Player(botName, playersID.Dequeue(), false, true, true);
                    newPlayer.AddEventCollectionChanged();
                    newPlayer.OnCardAddedToHand += PlayerAddedCard;
                    newPlayer.OnCardRemovedFromHand += PlayerRemovedCard;
                    newPlayer.SetBotPlayer(this, newPlayer, difficulty);
                    ConnectedServer.AddPlayer(newPlayer);

                    OnBotAdded.Invoke(newPlayer);
                }
                else
                    MessageBox.Show("Failed to add bot, lobby full");
            }
        }

        public void RemoveBot(Player player)
        {
            if (IsConnected && IsSinglePlayerMode)
            {
                ConnectedServer.RemovePlayer(player.ID);
                OnBotRemoved.Invoke(player.ID);
                playersID.Enqueue(player.ID);
            }
        }

        public void SetBotSettings(bool thinkEnabled)
        {
            BotPlayer.SimulateThinkTime = thinkEnabled;
        }

        public void SetServerState(ServerState state)
        {
            ConnectedServer.State = state;

            if (state == ServerState.InGame)
            {
                GameState.SilentSets = true;

                for (int index = 0; index < Rules.InitRules.Count; index++)
                    Rules.InitRules[index].InitState(this);

                GameState.SilentSets = false;
                IsGameInitialized = true;
            }
        }

        private Queue<int> playersID;

        private void PlayerAddedCard(object sender, Card card)
        {
            var player = sender as Player;
            OnAddedCard?.Invoke(player, player.Hand.Count, card != null ? true : false);
        }

        private void PlayerRemovedCard(object sender, Card card)
        {
            var player = sender as Player;
            OnRemovedCard?.Invoke(player, player.Hand.Count, card != null ? true : false);
        }
    }
}
