using System.Collections.Generic;
using System.IO;
using System.Net;

namespace DurakLibrary.Common
{
    public class ServerTag
    {
        public Dictionary<int, Player> Players { get; private set; } = new Dictionary<int, Player>();
        private string password;

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int SupportedPlayerCount { get; private set; }
        public string Password { get => password; set => SetPassword(value); }
        public bool PasswordProtected { get; private set; }

        public int PlayersCount { get; set; }
        public string SenderID { get; set; }
        public IPAddress IPAddress { get; set; }
        public int Port { get; set; }
        public IPEndPoint Address { get; set; }
        public ServerState State { get; set; }

        public ServerTag()
        {
        }

        public ServerTag(string name, string description, string password, int supportedPlayerCount)
        {
            Name = name;
            Description = description;
            SupportedPlayerCount = supportedPlayerCount;
            Password = password;
        }
        
        public void SetPassword(string plainTextPassword)
        {
            if (!string.IsNullOrEmpty(plainTextPassword))
            {
                password = plainTextPassword;
                PasswordProtected = true;
            }
            else
            {
                password = "Danil`";
                PasswordProtected = false;
            }
        }
        
        public void AddPlayer(Player player, bool addEvent = false)
        {
            if (addEvent)
                player.AddEventCollectionChanged();

            Players.Add(player.ID, player);
            PlayersCount++;
        }

        public void RemovePlayer(int playerID)
        {
            Players.Remove(playerID);
            PlayersCount--;
        }

        public void WriteToPacket(BinaryWriter writer, string senderID = null)
        {
            writer.Write(senderID);
            writer.Write(PlayersCount);
            writer.Write(SupportedPlayerCount);
            writer.Write((byte)State);
            writer.Write(Name);
            writer.Write(Description);
            writer.Write(password);
            writer.Write(PasswordProtected);
            writer.Write(Port);
            writer.Write(IPAddress.ToString());
        }

        public void WriteDataPlayer(BinaryWriter writer, Player player)
        {
            writer.Write(player.Name);
            writer.Write(player.ID);
            writer.Write(player.IsHost);
            writer.Write(player.IsReady);
            writer.Write(player.IsBot);
            writer.Write(player.IsDigress);
            writer.Write(player.IPAddress.ToString());
        }

        public void ReadFromPacket(BinaryReader reader)
        {
            SenderID = reader.ReadString();
            PlayersCount = reader.ReadInt32();
            SupportedPlayerCount = reader.ReadInt32();
            State = (ServerState)reader.ReadByte();
            Name = reader.ReadString();
            Description = reader.ReadString();
            Password = reader.ReadString();
            PasswordProtected = reader.ReadBoolean();
            Port = reader.ReadInt32();
            IPAddress = IPAddress.Parse(reader.ReadString());
            Address = new IPEndPoint(IPAddress, Port);
        }
        
        public Player ReadDataPlayer(BinaryReader reader, bool addEvent = false)
        {
            var player = new Player(
                    reader.ReadString(),
                    reader.ReadInt32(),
                    reader.ReadBoolean(),
                    reader.ReadBoolean(),
                    reader.ReadBoolean(),
                    reader.ReadBoolean())
            {IPAddress = IPAddress.Parse(reader.ReadString()) };

            AddPlayer(player, addEvent);
            return player;
        }
    }
}
