using DurakLibrary.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakLibrary.HostServer
{
    public class MainPlayerServer
    {
        public static ConcurrentDictionary<int, PlayerServer> PlayerServers { get; private set; }
        public static PlayerServer Host { get; private set; }
        public readonly int MaxPlayers;
        
        private TcpClient serverTcp;
        private Stream serverStreamReader;
        private Stream serverStreamWriter;
        private BinaryReader serverReader;
        private BinaryWriter serverWriter;
        private TcpListener tcpListenerPlayers;

        private Queue<int> playersID;
        private bool IsHostVisible = true;
        public int Port { get; private set; }
        public List<int> LeavingPlayers { get; private set; }

        public MainPlayerServer(int numPlayers = 6)
        {
            PlayerServers = new ConcurrentDictionary<int, PlayerServer>();
            LeavingPlayers = new List<int>();
            MaxPlayers = numPlayers;
        }

        public void Run()
        {
            CreatePlayersID();
            serverTcp = new TcpClient(NetUtils.GetAddress().ToString(), NetSettings.PORT_FOR_HOSTS);
            Task.Factory.StartNew(HandlingMessagesMainServer, TaskCreationOptions.LongRunning);

            var counter = 0;
            while (Port == 0)
            {
                Thread.Sleep(100);

                if (counter == 50)
                    throw new TimeoutException("Unable to get the port from the MainServer five seconds later");

                counter++;
            }
            
            Task.Run(GettingPlayers);
        }

        private void CreatePlayersID()
        {
            var shuffleNumbers = Enumerable.Range(0, MaxPlayers).OrderBy(n => Guid.NewGuid()).ToList();
            playersID = new Queue<int>(shuffleNumbers);
        }

        public void AddPlayer(int id, PlayerServer playerServer)
        {
            PlayerServers.TryAdd(id, playerServer);
        }

        public void RemovePlayer(int id)
        {
            if (PlayerServers.TryRemove(id, out _))
                playersID.Enqueue(id);

            if (Host.Core.ConnectedServer.State == ServerState.InGame)
                LeavingPlayers.Add(id);
            else
                Host.Core.ConnectedServer.RemovePlayer(id);
        }

        public void MakePlayerEternalBot(int id)
        {
            PlayerServers[id].IsPlayerRemoved = true;
            BroadcastRemovePlayer(id, PlayerServers[id]);
        }

        private void BroadcastRemovePlayer(int id, PlayerServer server)
        {
            var isLeave = true;
            var isBot = true;

            server.KickPlayerThisPlayerServer();
            RemovePlayer(id);

            foreach (var player in PlayerServers.Values)
                player.ReportPlayerDisconnected(id, isLeave, isBot);
        }

        public void GettingPlayers()
        {
            try
            {
                tcpListenerPlayers = new TcpListener(IPAddress.Any, Port);
                tcpListenerPlayers.Start();

                while (true)
                {
                    var tcpClient = tcpListenerPlayers.AcceptTcpClient();

                    if (!IsHostVisible || PlayerServers.Count >= MaxPlayers)
                    {
                        tcpClient.Close();
                        continue;
                    }

                    var isHost = playersID.Count == MaxPlayers;
                    var playerID = playersID.Dequeue();
                    var playerServer = new PlayerServer(this, tcpClient, playerID, isHost);
                    Task.Run(playerServer.HandlingMessagesPlayer);

                    if (isHost)
                        Host = PlayerServers[playerID];
                }
            }
            catch (Exception ex)
            {
                DisconnectPlayers();

                if (!(ex is InvalidOperationException || ex is SocketException))
                    MessageBox.Show($"Exception in MainPlayerServer!\nTargetSite:{ex.TargetSite}\n {ex.Message}\n StackTrace:{ex.StackTrace}");
            }
        }

        public void HandlingMessagesMainServer()
        {
            serverStreamReader = serverTcp.GetStream();
            serverStreamWriter = Stream.Synchronized(serverTcp.GetStream()); 
            serverReader = new BinaryReader(serverStreamReader);
            serverWriter = new BinaryWriter(serverStreamWriter);
            var block = true;

            while (block)
            {
                try
                {
                    var netMessageType = (NetMessageType)serverReader.ReadByte();

                    switch (netMessageType)
                    {
                        case NetMessageType.PlayingPort:
                            Port = serverReader.ReadInt32();
                            break;
                        case NetMessageType.DataHosts:
                            SendDataAboutHost();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    block = false;
                    //(-2146232800) Cannot read data from transport connection. The remote host forcibly terminated the existing connection.
                    if (!(ex is EndOfStreamException || ex is ObjectDisposedException || ex.HResult == -2146232800))
                        MessageBox.Show($"Exception in MainPlayerServer!\nTargetSite:{ex.TargetSite}\n {ex.Message}\n StackTrace:{ex.StackTrace}");
                }
            }
        }

        public void DisconnectPlayers()
        {
            tcpListenerPlayers.Stop();

            foreach (var player in PlayerServers?.Values)
                player.ClosePlayer();

            serverStreamReader.Close();
            serverTcp.Close();
        }

        public void SendDataAboutHost()
        {
            var senderID = serverReader.ReadString();
            serverWriter.Write((byte)NetMessageType.DataHosts);
            Host.Core.ConnectedServer.WriteToPacket(serverWriter, senderID);
        }

        public void SendHostVisibility()
        {
            IsHostVisible = !IsHostVisible;
            serverWriter.Write((byte)NetMessageType.HostVisibility);
            serverWriter.Write(IsHostVisible);
        }
    }
}
