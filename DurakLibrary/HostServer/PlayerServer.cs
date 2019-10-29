using DurakLibrary.Cards;
using DurakLibrary.Common;
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace DurakLibrary.HostServer
{
    public class PlayerServer
    {
        public static readonly Mutex Mutex = new Mutex();
        public readonly int PlayerID;

        public bool IsException { get; set; }
        public bool IsPlayerRemoved { get; set; }
        public ICollection<PlayerServer> PlayerServers { get => MainPlayerServer.PlayerServers.Values; }
        public CoreDurakGame Core { get => core ?? MainPlayerServer.Host.Core; }
        public Dictionary<int, Player> Players { get => Core.ConnectedServer.Players; }
        public bool IsHost { get => Players[PlayerID].IsHost; }

        public PlayerServer(MainPlayerServer mainPlayerServer, TcpClient playerTcp, int playerID, bool isHost)
        {
            this.mainPlayerServer = mainPlayerServer;
            this.playerTcp = playerTcp;
            PlayerID = playerID;

            messageHandlers = InitMessageHandlers();
            mainPlayerServer.AddPlayer(playerID, this);

            if (isHost)
            {
                core = new CoreDurakGame();
                Core.GameState.OnStateChanged += MyGameStateOnStateChanged;
            }
        }

        public void HandlingMessagesPlayer()
        {
            playerStreamReader = playerTcp.GetStream();
            playerStreamWriter = Stream.Synchronized(playerTcp.GetStream());
            playerReader = new BinaryReader(playerStreamReader);
            playerWriter = new BinaryWriter(playerStreamWriter);
            playerWriter.Write((byte)NetMessageType.PlayerID);
            playerWriter.Write(PlayerID);
            var block = true;
            var isMutexBlock = false;
            
            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)playerReader.ReadByte();
                    isMutexBlock = Mutex.WaitOne();

                    switch (netMessageType)
                    {
                        case NetMessageType.ConnectionApproval:
                            SendWelcomePackage();
                            AddPlayer();
                            break;
                        case NetMessageType.RequestRemovePlayer:
                            RemovePlayer();
                            break;
                        case NetMessageType.ServerStateChanged:
                            SetServerState((ServerState)playerReader.ReadByte());
                            break;
                        case NetMessageType.Data:
                            HandleMessage();
                            break;
                    }
                    Mutex.ReleaseMutex();
                    isMutexBlock = false;
                }
                catch (Exception ex)
                {
                    block = false;
                    IsException = true;
                    HandleRemovingPlayer(PlayerID);

                    if (!(ex is EndOfStreamException || ex is ObjectDisposedException || ex.HResult == -2146232800))
                        MessageBox.Show($"Exception in PlayerServer!\nTargetSite:{ex.TargetSite}\n {ex.Message}\n StackTrace:{ex.StackTrace}");
                    
                    if (isMutexBlock)
                        Mutex.ReleaseMutex();
                }
            }
        }

        public void ClosePlayer()
        {
            if (!IsPlayerRemoved)
                mainPlayerServer.RemovePlayer(PlayerID);

            playerStreamWriter?.Close();
            playerTcp?.Close();
        }

        public void KickPlayerThisPlayerServer()
        {
            if (playerWriter.BaseStream.CanWrite && !IsException)
            {
                playerWriter.Write((byte)NetMessageType.PlayerKicked);
                playerWriter.Write("You have been kicked by host");
            }
        }

        public void ReportPlayerDisconnected(int id, bool isLeave, bool isBot)
        {
            playerWriter.Write((byte)NetMessageType.Data);
            playerWriter.Write((byte)MessageType.PlayerDisconnected);
            playerWriter.Write(id);
            playerWriter.Write(isLeave);
            playerWriter.Write(isBot);
        }

        private readonly Dictionary<MessageType, Action> messageHandlers;
        private readonly CoreDurakGame core; //Access only through the property Core.
        private MainPlayerServer mainPlayerServer;
        private TcpClient playerTcp;
        private Stream playerStreamReader;
        private Stream playerStreamWriter;
        private BinaryReader playerReader;
        private BinaryWriter playerWriter;

        private Dictionary<MessageType, Action> InitMessageHandlers()
        {
            return new Dictionary<MessageType, Action>
            {
                { MessageType.PlayerIsReady, HandlePlayerReadiness },
                { MessageType.PlayerChat, HandlePlayerChat },
                { MessageType.RequestState, HandleStateRequest },
                { MessageType.HostReqStart, HandleHostReqStart },
                { MessageType.SendMove, HandleGameMove },
                { MessageType.PlayerDigressed, HandlePlayerDigressed }
            };
        }

        private void SendWelcomePackage()
        {
            playerWriter.Write((byte)NetMessageType.Data);
            playerWriter.Write((byte)MessageType.WelcomePackage);
            playerWriter.Write(Players.Count);

            foreach (var player in Players.Values)
                Core.ConnectedServer.WriteDataPlayer(playerWriter, player);
        }

        private void HandleMessage()
        {
            var messageType = (MessageType)playerReader.ReadByte();

            if (messageHandlers.ContainsKey(messageType))
                messageHandlers[messageType].Invoke();
        }

        private void AddPlayer()
        {
            var newPlayer = Core.ConnectedServer.ReadDataPlayer(playerReader, true);
            newPlayer.SetBotPlayer(Core, newPlayer, 1);
            newPlayer.OnCardAddedToHand += PlayerAddedCard;
            newPlayer.OnCardRemovedFromHand += PlayerRemovedCard;

            foreach (var p in PlayerServers)
            {
                p.playerWriter.Write((byte)NetMessageType.Data);
                p.playerWriter.Write((byte)MessageType.PlayerConnected);
                Core.ConnectedServer.WriteDataPlayer(p.playerWriter, newPlayer);
            }
        }

        private void RemovePlayer()
        {
            if (IsHost)
            {
                var id = playerReader.ReadInt32();
                HandleRemovingPlayer(id);
            }
        }

        private void HandleRemovingPlayer(int id)
        {
            if (MainPlayerServer.PlayerServers.ContainsKey(id) && !MainPlayerServer.PlayerServers[id].IsPlayerRemoved)
            {
                mainPlayerServer.MakePlayerEternalBot(id);
                RunOrStopBots();

                if (Players.ContainsKey(id))
                    Players[id].IsBot = true;
            }
        }

        private void HandlePlayerReadiness()
        {
            var isReady = playerReader.ReadBoolean();
            Players[PlayerID].IsReady = isReady;
            BroadcastPlayerReadiness(isReady, PlayerID);
        }

        private void BroadcastPlayerReadiness(bool isReady, int id)
        {
            foreach (var p in PlayerServers)
            {
                p.playerWriter.Write((byte)NetMessageType.Data);
                p.playerWriter.Write((byte)MessageType.PlayerIsReady);
                p.playerWriter.Write(id);
                p.playerWriter.Write(isReady);
            }
        }

        private void HandlePlayerChat()
        {
            var message = playerReader.ReadString();

            foreach (var p in PlayerServers)
            {
                p.playerWriter.Write((byte)NetMessageType.Data);
                p.playerWriter.Write((byte)MessageType.PlayerChat);
                p.playerWriter.Write(PlayerID);
                p.playerWriter.Write(message);
            }
        }

        private void HandleStateRequest()
        {
            if (IsHost)
            {
                var parameter = StateParameter.CreateEmpty();
                parameter.Decode(playerReader);

                for (int index = 0; index < Rules.ClientStateReqValidators.Count; index++)
                    Rules.ClientStateReqValidators[index].TrySetState(parameter, Core, Players[PlayerID]);
            }
        }

        private void HandleHostReqStart()
        {
            var isStart = playerReader.ReadBoolean();

            if (IsHost && isStart)
            {
                var isLobbyReady = true;
                var notReady = Players.Values.Where(p => !p.IsReady).ToArray();
                isLobbyReady = notReady.Length == 0;

                if (isLobbyReady)
                    SetServerState(ServerState.InGame);
                else
                {
                    var message = "Cannot start, following players not ready:\n";

                    foreach (var player in notReady)
                        message += "\t" + player.Name + "\n";

                    foreach (var p in PlayerServers)
                    {
                        p.playerWriter.Write((byte)NetMessageType.Data);
                        p.playerWriter.Write((byte)MessageType.CannotStart);
                        p.playerWriter.Write(message);
                    }
                }
            }
        }

        private void SetServerState(ServerState state)
        {
            if (IsHost && Core.ConnectedServer.State != state)
            {
                Core.ConnectedServer.State = state;

                foreach (var p in PlayerServers)
                {
                    p.playerWriter.Write((byte)NetMessageType.ServerStateChanged);
                    p.playerWriter.Write((byte)state);
                }

                if (state == ServerState.InGame)
                {
                    mainPlayerServer.SendHostVisibility();
                    HandleServerStateInGame();
                }
                else if (state == ServerState.InLobby)
                {
                    mainPlayerServer.SendHostVisibility();
                    HandleServerStateInLobby();
                }
            }
        }

        private void HandleServerStateInGame()
        {
            Core.GameState.SilentSets = true;

            for (int index = 0; index < Rules.InitRules.Count; index++)
                Rules.InitRules[index].InitState(Core);

            Core.GameState.SilentSets = false;

            foreach (var p in PlayerServers)
            {
                p.playerWriter.Write((byte)NetMessageType.Data);
                p.playerWriter.Write((byte)MessageType.FullGameStateTransfer);
                Core.GameState.Encode(p.playerWriter);
            }
            //Before the bots begin to move very quickly, it is necessary that the game was drawn by other players
            Thread.Sleep(3000); 
            Core.IsGameInitialized = true;
        }

        private void HandleServerStateInLobby()
        {
            foreach (var player in Players.Values)
            {
                if (!player.IsBot && !player.IsHost)
                    player.IsReady = false;

                player.ProposedMoves.Clear();
                player.Hand.Clear();

                BroadcastPlayerReadiness(player.IsReady, player.ID);
            }

            HandleLeavingPlayers();
            Core.GameState.Clear();
            Core.IsGameInitialized = false;
        }

        private void HandleLeavingPlayers()
        {
            var removingPlayers = mainPlayerServer.LeavingPlayers;
            var isRemove = removingPlayers.Count != 0;

            if (isRemove)
            {
                foreach (var p in PlayerServers)
                {
                    p.playerWriter.Write((byte)NetMessageType.Data);
                    p.playerWriter.Write((byte)MessageType.RemovePlayers);
                    p.playerWriter.Write(removingPlayers.Count());
                    removingPlayers.ForEach(id => p.playerWriter.Write(id));
                }

                removingPlayers.ForEach(id => Core.ConnectedServer.RemovePlayer(id));
                removingPlayers.Clear();
            }
        }

        private void MyGameStateOnStateChanged(object sender, StateParameter parameter)
        {
            if (Core.ConnectedServer.State == ServerState.InGame && parameter.IsSynced)
            {
                foreach (var p in PlayerServers)
                {
                    p.playerWriter.Write((byte)NetMessageType.Data);
                    p.playerWriter.Write((byte)MessageType.GameStateChanged);
                    parameter.Encode(p.playerWriter);
                }
            }
        }

        private void PlayerRemovedCard(object sender, Card card)
        {
            var player = sender as Player;
            NotifyNewCardState(player, card, false);
        }

        private void PlayerAddedCard(object sender, Card card)
        {
            var player = sender as Player;
            NotifyNewCardState(player, card, true);
        }

        private void NotifyNewCardState(Player player, Card card, bool added)
        {
            if (MainPlayerServer.PlayerServers.ContainsKey(player.ID))
            {
                var writer = MainPlayerServer.PlayerServers[player.ID].playerWriter;
                writer.Write((byte)NetMessageType.Data);
                writer.Write((byte)MessageType.PlayerHandChanged);
                writer.Write(added);
                writer.Write(card != null);

                if (card != null)
                {
                    writer.Write((byte)card.Value);
                    writer.Write((byte)card.Suit);
                }
            }

            BroadcastCardCountChanged(player, card);
        }

        private void BroadcastCardCountChanged(Player player, Card card)
        {
            foreach (var p in PlayerServers)
            {
                p.playerWriter.Write((byte)NetMessageType.Data);
                p.playerWriter.Write((byte)MessageType.CardCountChanged);
                p.playerWriter.Write(player.ID);
                p.playerWriter.Write(player.Hand.Count);
                p.playerWriter.Write(card != null ? true : false);
            }
        }

        private void HandleGameMove()
        {
            var move = GameMove.Decode(playerReader, Players);

            if (Core.ConnectedServer.State == ServerState.InGame && move.Player == Players[PlayerID])
                Core.HandleMove(move, playerWriter);
        }

        private void HandlePlayerDigressed()
        {
            RunOrStopBots();

            var isDigress = playerReader.ReadBoolean();
            var isBot = playerReader.ReadBoolean();
            Players[PlayerID].IsDigress = isDigress;
            Players[PlayerID].IsBot = isBot;

            foreach (var p in PlayerServers)
            {
                p.playerWriter.Write((byte)NetMessageType.Data);
                p.playerWriter.Write((byte)MessageType.PlayerDigressed);
                p.playerWriter.Write(PlayerID);
                p.playerWriter.Write(isDigress);
                p.playerWriter.Write(isBot);
            }
        }

        private void RunOrStopBots()
        {
            if (!Players.Values.Any(p => p.IsBot))
                Core.RunBots();
            else if (Players.Values.All(p => !p.IsBot))
                Core.StopBots();
        }
    }
}

