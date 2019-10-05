using DurakLibrary.Cards;
using System.Collections.Generic;
using System.IO;

namespace DurakLibrary.Common
{
    public struct GameMove
    {
        private Player player;
        private Card move;
        public Player Player { get => player; }
        public Card Move { get => move; }

        public GameMove(Player player, Card move) : this()
        {
            this.player = player;
            this.move = move;
        }

        public void Encode(BinaryWriter writer)
        {
            writer.Write(player.ID);
            writer.Write(move != null);

            if (move != null)
            {
                writer.Write((byte)move.Value);
                writer.Write((byte)move.Suit);
            }
        }

        public static GameMove DecodeFromClient(BinaryReader writer, Dictionary<int, Player> players)
        {
            var result = new GameMove();
            var playerID = writer.ReadInt32();
            result.player = players[playerID];
            var hasCardValue = writer.ReadBoolean();

            if (hasCardValue)
            {
                var moveValue = writer.ReadByte();
                var moveSuit = writer.ReadByte();
                result.move = new Card((CardValue)moveValue, (CardSuit)moveSuit);
                result.move.FaceUp = true;
            }

            return result;
        }

        public static GameMove Decode(BinaryReader reader, Dictionary<int, Player> players)
        {
            var result = new GameMove();
            var playerID = reader.ReadInt32();
            var hasCardValue = reader.ReadBoolean();
            result.player = players[playerID];

            if (hasCardValue)
            {
                var moveValue = reader.ReadByte();
                var moveSuit = reader.ReadByte();

                result.move = new Card((CardValue)moveValue, (CardSuit)moveSuit);
                result.move.FaceUp = true;
            }

            return result;
        }
    }
}
