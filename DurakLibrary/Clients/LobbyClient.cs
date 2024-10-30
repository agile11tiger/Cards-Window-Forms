using DurakLibrary.Cards;
using DurakLibrary.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakLibrary.Clients
{
    public class LobbyClient : CoreDurakGame
    {
        public event Action OnConnected;
        public event Action<Player> OnPlayerConnected;
        public event Action<int> OnPlayerDisconnectedLobby;
        public event Action<int, bool, bool> OnPlayerDisconnectedGame;
        public event Action OnPlayerKicked;
        public event Action<ServerState> OnServerStateUpdated;
        public event Action<int, bool> OnPlayerIsReady;
        public event Action<int, string> OnPlayerChatLobby;
        public event Action<string> OnCannotStart;
        public event Action<Player, int, bool> OnPlayerCardCountChanged;
        public event Action<Card, string> OnInvalidMove;
        public event Action<Player, string> OnPlayerChatGame;
        public event Action<int, bool, bool> OnPlayerDigressed;
        public event Action<string> OnLobbyClose;
        public event Action OnReturnToLobby;
        public event Action OnGameClose;

        public bool IsGameClosing { get; set; }
        public bool IsLobbyClosing { get; set; }
        public VoiceChat Chat { get; set; }

        public LobbyClient(Player player) : base(player)
        {
            messageHandlers = new Dictionary<MessageType, Action>
            {
                { MessageType.WelcomePackage, HandleWelcomePackage },
                { MessageType.PlayerConnected, HandlePlayerConnected },
                { MessageType.PlayerDisconnected, HandlePlayerDisconnected },
                { MessageType.PlayerIsReady, HandleReadiness },
                { MessageType.PlayerChat, HandlePlayerChat },
                { MessageType.CannotStart, HandleCannotStart },
                { MessageType.FullGameStateTransfer, HandleGameStateReceived },
                { MessageType.GameStateChanged, HandleStateChanged },
                { MessageType.PlayerHandChanged, HandleCardChanged },
                { MessageType.CardCountChanged, HandleCardCountChanged },
                { MessageType.InvalidMove, HandleInvalidMove },
                { MessageType.PlayerDigressed, HandlePlayerDigressed },
                { MessageType.RemovePlayers, HandleRemoveAllBots }
            };
        }

        public void SetLobbyTcp(IPAddress ip, int port)
        {
            lobbyTcp = new TcpClient();
            lobbyTcp.Connect(ip, port);
        }

        public async void RunLobbyClient()
        {
            var guiContext = SynchronizationContext.Current;
            await Task.Factory.StartNew(() => HandlingMessagesLobby(guiContext), TaskCreationOptions.LongRunning);
        }

        public void ConnectHost()
        {
            lobbyWriter.Write((byte)NetMessageType.ConnectionApproval);
            ConnectedServer.WriteDataPlayer(lobbyWriter, PlayerUntill);
        }

        public void ConnectTo(ServerTag host)
        {
            if (ConnectedServer == null)
            {
                SetLobbyTcp(PlayerUntill.IPAddress, host.Port);
                RunLobbyClient();
                CheckConnection(ref lobbyWriter);

                try
                {
                    ConnectedServer = host;
                    ConnectedServer.PlayersCount = 0;
                    lobbyWriter.Write((byte)NetMessageType.ConnectionApproval);
                    ConnectedServer.WriteDataPlayer(lobbyWriter, PlayerUntill);
                }
                catch
                {
                    ConnectedServer = null;
                    MessageBox.Show("You cannot connect to this host");
                    return;
                }

                OnConnected.Invoke();
            }
            else
                throw new InvalidOperationException("Cannot connect when this client is already connected");
        }

        //Perhaps it’s bad that I used "ref", but otherwise "writer" will null.
        //And I do not want to write two identical methods)
        public void CheckConnection(ref BinaryWriter writer)
        {
            var counter = 0;

            while (writer == null)
            {
                Thread.Sleep(100);

                if (counter >= 50)
                    throw new TimeoutException("Connection failed. Writer was null five seconds later");

                counter++;
            }
        }

        public void PrepareToReturnToLobby()
        {
            IsGameInitialized = false;
            GameState.Clear();
            ConnectedServer.Players[Player.ID].Hand.Clear();

            OnReturnToLobby.Invoke();
            IsGameClosing = false;
        }

        public void CloseLobby(object reason)
        {
            if (!IsGameClosing)
                OnGameClose?.Invoke();

            if (!IsLobbyClosing)
                OnLobbyClose?.Invoke((string)reason);

            Chat?.CloseChat();
            lobbyStream?.Close();
            lobbyTcp?.Close();
        }

        #region Message Writers
        public void RequestKick(Player player)
        {
            if (Player.IsHost)
            {
                lobbyWriter.Write((byte)NetMessageType.RequestRemovePlayer);
                lobbyWriter.Write(player.ID);
            }
        }

        public void ReadyClicked(bool isReady)
        {
            lobbyWriter.Write((byte)NetMessageType.Data);
            lobbyWriter.Write((byte)MessageType.PlayerIsReady);
            lobbyWriter.Write(isReady);
        }

        public void SendChatMessage(string message)
        {
            if (message.Length > 30)
                message = message.Remove(30);

            lobbyWriter.Write((byte)NetMessageType.Data);
            lobbyWriter.Write((byte)MessageType.PlayerChat);
            lobbyWriter.Write(message);
        }

        public void RequestState(StateParameter parameter)
        {
            if (Player.IsHost)
            {
                lobbyWriter.Write((byte)NetMessageType.Data);
                lobbyWriter.Write((byte)MessageType.RequestState);
                parameter.Encode(lobbyWriter);
            }
        }

        public void RequestStart()
        {
            if (Player.IsHost)
            {
                lobbyWriter.Write((byte)NetMessageType.Data);
                lobbyWriter.Write((byte)MessageType.HostReqStart);
                lobbyWriter.Write(true);
            }
        }

        public void RequestMove(Card card)
        {
            var move = new GameMove(Player, card);
            lobbyWriter.Write((byte)NetMessageType.Data);
            lobbyWriter.Write((byte)MessageType.SendMove);
            move.Encode(lobbyWriter);
        }

        public void RequestServerState(ServerState state)
        {
            if (Player.IsHost)
            {
                lobbyWriter.Write((byte)NetMessageType.ServerStateChanged);
                lobbyWriter.Write((byte)state);
            }
        }

        public void RequestDigress(bool isDigress, bool isBot)
        {
            lobbyWriter.Write((byte)NetMessageType.Data);
            lobbyWriter.Write((byte)MessageType.PlayerDigressed);
            lobbyWriter.Write(isDigress);
            lobbyWriter.Write(isBot);
        }
        #endregion

        private Dictionary<MessageType, Action> messageHandlers;
        private TcpClient lobbyTcp;
        private NetworkStream lobbyStream;
        private BinaryReader lobbyReader;
        private BinaryWriter lobbyWriter;

        private void HandlingMessagesLobby(SynchronizationContext guiContext)
        {
            lobbyStream = lobbyTcp.GetStream();
            lobbyReader = new BinaryReader(lobbyStream);
            lobbyWriter = new BinaryWriter(lobbyStream);
            var block = true;

            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)lobbyReader.ReadByte();

                    switch (netMessageType)
                    {
                        case NetMessageType.PlayerID:
                            PlayerUntill.ID = lobbyReader.ReadInt32();
                            break;
                        case NetMessageType.PlayerKicked:
                            block = false;
                            var reason = lobbyReader.ReadString();
                            guiContext.Send(CloseLobby, reason);
                            break;
                        case NetMessageType.ServerStateChanged:
                            guiContext.Post(HandleServerStateReceived, (ServerState)lobbyReader.ReadByte());
                            break;
                        case NetMessageType.Data:
                            guiContext.Send(HandleMessage, null);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    block = false;
                    guiContext.Send(CloseLobby, null);

                    //(-2146232800) Cannot read data from transport connection. The remote host forcibly terminated the existing connection.
                    if (!(ex is EndOfStreamException || ex is ObjectDisposedException || ex.HResult == -2146232800))
                        MessageBox.Show($"Exception in LobbyClient!\n {ex.Message}\n StackTrace:{ex.StackTrace}");
                }
            }
        }

        #region Message Handlers
        private void HandleMessage(object obj)
        {
            var messageType = (MessageType)lobbyReader.ReadByte();

            if (messageHandlers.ContainsKey(messageType))
                messageHandlers[messageType].Invoke();
        }

        private void HandleWelcomePackage()
        {
            var playersCount = lobbyReader.ReadInt32();

            for (var i = 0; i < playersCount; i++)
                HandlePlayerConnected();
        }

        private void HandlePlayerConnected()
        {
            var player = ConnectedServer.ReadDataPlayer(lobbyReader);

            if (!Chat.MutedPlayers.ContainsKey(player.IPAddress))
                Chat.MutedPlayers.Add(player.IPAddress, false);

            OnPlayerConnected.Invoke(player);
        }

        private void HandlePlayerDisconnected()
        {
            var playerID = lobbyReader.ReadInt32();
            var isLeave = lobbyReader.ReadBoolean();
            var isBot = lobbyReader.ReadBoolean();
            Chat.MutedPlayers.Remove(ConnectedServer.Players[playerID].IPAddress);

            if (ConnectedServer.State == ServerState.InGame)
            {
                ConnectedServer.Players[playerID].IsBot = isBot;
                OnPlayerDisconnectedGame.Invoke(playerID, isLeave, isBot);
                OnPlayerKicked.Invoke();
            }
            else
            {
                ConnectedServer.RemovePlayer(playerID);
                OnPlayerDisconnectedLobby.Invoke(playerID);
            }
        }

        private void HandleReadiness()
        {
            var id = lobbyReader.ReadInt32();
            var isReady = lobbyReader.ReadBoolean();
            ConnectedServer.Players[id].IsReady = isReady;

            if (!ConnectedServer.Players[id].IsHost)
                OnPlayerIsReady.Invoke(id, isReady);
        }

        private void HandlePlayerChat()
        {
            var id = lobbyReader.ReadInt32();
            var message = lobbyReader.ReadString();

            if (ConnectedServer.State == ServerState.InGame)
                OnPlayerChatGame.Invoke(ConnectedServer.Players[id], message);
            else
                OnPlayerChatLobby.Invoke(id, message);
        }

        private void HandleCannotStart()
        {
            var message = lobbyReader.ReadString();
            OnCannotStart.Invoke(message);
        }

        private void HandleServerStateReceived(object state)
        {
            var serverState = (ServerState)state;
            ConnectedServer.State = serverState;
            OnServerStateUpdated.Invoke(serverState);
        }

        private void HandleGameStateReceived()
        {
            GameState.Decode(lobbyReader);
        }

        private void HandleStateChanged()
        {
            StateParameter.Decode(lobbyReader, GameState);
        }

        private void HandleCardChanged()
        {
            var added = lobbyReader.ReadBoolean();
            var hasValue = lobbyReader.ReadBoolean();

            if (hasValue)
            {
                var rank = (CardValue)lobbyReader.ReadByte();
                var suit = (CardSuit)lobbyReader.ReadByte();
                var card = new Card(rank, suit) { FaceUp = true };

                if (added)
                    Player.Hand.Add(card);
                else
                    Player.Hand.Remove(card);
            }
        }

        private void HandleCardCountChanged()
        {
            var playerId = lobbyReader.ReadInt32();
            var newCardsCount = lobbyReader.ReadInt32();
            var isCardCountChanged = lobbyReader.ReadBoolean();
            OnPlayerCardCountChanged.Invoke(ConnectedServer.Players[playerId], newCardsCount, isCardCountChanged);
        }

        private void HandleInvalidMove()
        {
            var reason = lobbyReader.ReadString();
            var move = GameMove.Decode(lobbyReader, ConnectedServer.Players);
            OnInvalidMove.Invoke(move.Move, reason);
        }

        private void HandlePlayerDigressed()
        {
            var playerID = lobbyReader.ReadInt32();
            var isDigress = lobbyReader.ReadBoolean();
            var isBot = lobbyReader.ReadBoolean();
            ConnectedServer.Players[playerID].IsDigress = isDigress;
            ConnectedServer.Players[playerID].IsBot = isBot;
            OnPlayerDigressed.Invoke(playerID, isDigress, isBot);
        }

        private void HandleRemoveAllBots()
        {
            var countPlayers = lobbyReader.ReadInt32();

            for (var i = 0; i < countPlayers; i++)
            {
                var playerID = lobbyReader.ReadInt32();
                ConnectedServer.RemovePlayer(playerID);
                OnPlayerDisconnectedLobby.Invoke(playerID);
            }
        }
        #endregion
    }
}
