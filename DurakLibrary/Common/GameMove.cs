using DurakLibrary.Cards;
using System.Collections.Generic;
using System.IO;

namespace DurakLibrary.Common
{
    public struct GameMove
    {
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

        private Player player;
        private Card move;
    }
}
