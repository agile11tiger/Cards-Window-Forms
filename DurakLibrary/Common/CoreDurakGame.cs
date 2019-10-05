using DurakLibrary.Cards;
using DurakLibrary.Properties;
using DurakLibrary.HostServer;
using System.IO;
using System.Linq;
using System.Threading;

namespace DurakLibrary.Common
{
    public class CoreDurakGame
    {
        private Timer timer;
        public readonly string[] BotNames;

        public ServerTag ConnectedServer { get; set; }
        public GameState GameState { get; private set; }
        public Player PlayerUntill { get; private set; } // Used before connecting to the server 
        public Player Player { get => ConnectedServer.Players[PlayerUntill.ID]; }
        public bool IsConnected { get => ConnectedServer != null; }
        public bool IsGameInitialized { get; set; }
        public bool IsSinglePlayerMode { get; set; }

        public CoreDurakGame()
        {
            BotNames = Resources.names_corrected.ToLower().Split('\n');
            GameState = new GameState();
        }

        public CoreDurakGame(Player player) : this()
        {
            PlayerUntill = player;
        }

        public void RunBots(SynchronizationContext sync = null)
        {
            timer = new Timer(Tick, sync, 3000, 3000);
        }

        public void StopBots()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
        }

        private void Tick(object state)
        {
            var sync = state as SynchronizationContext;

            if (sync != null)
                sync.Send(BotTriesMove, null);
            else
                BotTriesMove(null);
        }

        public void BotTriesMove(object state)
        {
            if (ConnectedServer.State == ServerState.InGame && IsGameInitialized)
            {
                PlayerServer.Mutex?.WaitOne();
                foreach (var botPlayer in ConnectedServer.Players.Values.Where(p => p.IsBot))
                {
                    if (botPlayer.ShouldMove())
                    {
                        Card move = botPlayer.DetermineMove();
                        HandleMove(new GameMove(botPlayer.Player, move));
                    }
                }
                PlayerServer.Mutex?.ReleaseMutex();
            }
        }

        public void HandleMove(GameMove move, BinaryWriter writer = null)
        {
            var failReason = "Unknown";

            for (int index = 0; index < Rules.PlayRules.Count; index++)
            {
                if (!Rules.PlayRules[index].IsValidMove(this, move, ref failReason))
                {
                    if (!move.Player.IsBot)
                    {
                        if (failReason == "Unknown")
                            failReason = "Failed on " + Rules.PlayRules[index].ReadableName;

                        if (IsSinglePlayerMode)
                            System.Windows.Forms.MessageBox.Show(failReason, "Alert", System.Windows.Forms.MessageBoxButtons.OK);
                        else
                            NotifyInvalidMove(move, failReason, writer);
                    }

                    return;
                }
            }

            CheckMoveRules(move);
            CheckStateRules();
        }

        private void NotifyInvalidMove(GameMove move, string reason, BinaryWriter writer)
        {
            writer.Write((byte)NetMessageType.Data);
            writer.Write((byte)MessageType.InvalidMove);
            writer.Write(reason);
            move.Encode(writer);
        }

        public void CheckMoveRules(GameMove move)
        {
            foreach (var moveRule in Rules.MoveSuccessRules)
                moveRule.UpdateState(this, move);
        }

        public void CheckStateRules()
        {
            foreach (var stateRule in Rules.StateRules)
                stateRule.ValidateState(this);
        }

        public bool CanPlayMove(Player player, Card card)
        {
            var move = new GameMove(player, card);
            var failReason = "";

            foreach (var playRule in Rules.PlayRules)
                if (!playRule.IsValidMove(this, move, ref failReason))
                    return false;

            return true;
        }
    }
}
