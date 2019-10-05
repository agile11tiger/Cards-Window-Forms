using DurakLibrary.Cards;
using DurakLibrary.Clients;
using DurakLibrary.Common;
using DurakLibrary.HostServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class Game : Form
    {
        private struct PlayerUITag
        {
            public BorderPanel Panel;
            public Label Win;
            public Label Name;
            public Label PlayerID;
            public Label CardCount;
            public PictureBox Card;
            public PictureBox MutedPlayer;
            public PictureBox SwordShieldDagger;
            public PictureBox BotGame;
            public PictureBox Digress;
            public PictureBox Leave;
            public RadioButton RadioButton;
            public Button Kick;
        }

        private CoreDurakGame core;
        private LobbyClient lobbyClient;
        private SingleClientServer singleServer;
        private Dictionary<Player, PlayerUITag> playerUIs;
        private List<RadioButton> radioButtons;
        private bool isMicroActivated;

        public Game()
        {
            InitializeComponent();
            playerUIs = new Dictionary<Player, PlayerUITag>();
            radioButtons = new List<RadioButton> { rbnPlayer1, rbnPlayer2, rbnPlayer3, rbnPlayer4, rbnPlayer5 };
        }

        public void SetClient(CoreDurakGame coreGame)
        {
            core = coreGame;
            AddEvents();

            foreach (var player in core.ConnectedServer.Players.Values)
            {
                if (player != null && player.ID != core.Player.ID)
                {
                    var id = player.ID == 0 ? core.Player.ID : player.ID;
                    var tag = new PlayerUITag();

                    switch (id)
                    {
                        case 1:
                            tag.Panel = pnlPlayer1;
                            tag.Name = lblPlayer1;
                            tag.Name.Text = player.Name;
                            tag.PlayerID = lblIDPlayer1;
                            tag.PlayerID.Text = player.ID.ToString();
                            tag.CardCount = lblCardsLeftPlayer1;
                            tag.CardCount.Text = player.Hand.Count.ToString();
                            tag.Card = pbxPlayer1;
                            tag.Kick = btnKickPlayer1;
                            tag.BotGame = pbxBotGamePlayer1;
                            tag.Digress = pbxDigressedPlayer1;
                            tag.Leave = pbxLeavedPlayer1;
                            tag.Win = lblWinPlayer1;
                            tag.MutedPlayer = pbxMutedPlayer1;
                            tag.RadioButton = rbnPlayer1;
                            break;
                        #region DRY case 2-5
                        case 2:
                            tag.Panel = pnlPlayer2;
                            tag.Name = lblPlayer2;
                            tag.Name.Text = player.Name;
                            tag.PlayerID = lblIDPlayer2;
                            tag.PlayerID.Text = player.ID.ToString();
                            tag.CardCount = lblCardsLeftPlayer2;
                            tag.CardCount.Text = player.Hand.Count.ToString();
                            tag.Card = pbxPlayer2;
                            tag.Kick = btnKickPlayer2;
                            tag.BotGame = pbxBotGamePlayer2;
                            tag.Digress = pbxDigressedPlayer2;
                            tag.Leave = pbxLeavedPlayer2;
                            tag.Win = lblWinPlayer2;
                            tag.MutedPlayer = pbxMutedPlayer2;
                            tag.RadioButton = rbnPlayer2;
                            break;
                        case 3:
                            tag.Panel = pnlPlayer3;
                            tag.Name = lblPlayer3;
                            tag.Name.Text = player.Name;
                            tag.PlayerID = lblIDPlayer3;
                            tag.PlayerID.Text = player.ID.ToString();
                            tag.CardCount = lblCardsLeftPlayer3;
                            tag.CardCount.Text = player.Hand.Count.ToString();
                            tag.Card = pbxPlayer3;
                            tag.Kick = btnKickPlayer3;
                            tag.BotGame = pbxBotGamePlayer3;
                            tag.Digress = pbxDigressedPlayer3;
                            tag.Leave = pbxLeavedPlayer3;
                            tag.Win = lblWinPlayer3;
                            tag.MutedPlayer = pbxMutedPlayer3;
                            tag.RadioButton = rbnPlayer3;
                            break;
                        case 4:
                            tag.Panel = pnlPlayer4;
                            tag.Name = lblPlayer4;
                            tag.Name.Text = player.Name;
                            tag.PlayerID = lblIDPlayer4;
                            tag.PlayerID.Text = player.ID.ToString();
                            tag.CardCount = lblCardsLeftPlayer4;
                            tag.CardCount.Text = player.Hand.Count.ToString();
                            tag.Card = pbxPlayer4;
                            tag.Kick = btnKickPlayer4;
                            tag.BotGame = pbxBotGamePlayer4;
                            tag.Digress = pbxDigressedPlayer4;
                            tag.Leave = pbxLeavedPlayer4;
                            tag.Win = lblWinPlayer4;
                            tag.MutedPlayer = pbxMutedPlayer4;
                            tag.RadioButton = rbnPlayer4;
                            break;
                        case 5:
                            tag.Panel = pnlPlayer5;
                            tag.Name = lblPlayer5;
                            tag.Name.Text = player.Name;
                            tag.PlayerID = lblIDPlayer5;
                            tag.PlayerID.Text = player.ID.ToString();
                            tag.CardCount = lblCardsLeftPlayer5;
                            tag.CardCount.Text = player.Hand.Count.ToString();
                            tag.Card = pbxPlayer5;
                            tag.Kick = btnKickPlayer5;
                            tag.BotGame = pbxBotGamePlayer5;
                            tag.Digress = pbxDigressedPlayer5;
                            tag.Leave = pbxLeavedPlayer5;
                            tag.Win = lblWinPlayer5;
                            tag.MutedPlayer = pbxMutedPlayer5;
                            tag.RadioButton = rbnPlayer5;
                            break;
                            #endregion
                    }

                    playerUIs.Add(player, tag);
                    tag.Panel.Visible = true;
                }
            }

            var myTag = new PlayerUITag()
            {
                PlayerID = lblIDPlayer,
                Panel = pnlMyView,
                SwordShieldDagger = pbxSwordShieldDagger,
                BotGame = pbxBotGame,
                Digress = pbxDigressed,
                Win = lblWinPlayer
            };
            myTag.PlayerID.Text = core.Player.ID.ToString();
            playerUIs.Add(core.Player, myTag);

            cplPlayersHand.Player = core.Player;
            cplPlayersHand.UpdatePlayer();
            DetermineKickButtons();
            core.ConnectedServer.Players
                .Where(p => p.Value.IsBot && p.Value.IsDigress)
                .ToList()
                ?.ForEach(p => PlayerDigressed(p.Key, p.Value.IsBot, p.Value.IsDigress));
        }

        private void AddEvents()
        {
            AddCardEvents();
            AddDeckEvents();
            AddDiscardEvents();
            AddDefenderEvents();
            AddRemainEvents();
        }

        private void AddCardEvents()
        {
            // Refresh is needed so that OnPaint() draws immediately, otherwise the attacking card will be above the defending card 
            // when the bots make too fast moves.(the correct order of adding to Control, as well as BringToFront () do not help)
            core.GameState.AddStateChangedEvent("attacking_card", 0, (X, Y) => { cbxPlayerAttack.Card = Y.GetValueCard(); cbxPlayerAttack.Refresh(); });
            core.GameState.AddStateChangedEvent("attacking_card", 1, (X, Y) => { cbxPlayerAttack1.Card = Y.GetValueCard(); cbxPlayerAttack1.Refresh(); });
            core.GameState.AddStateChangedEvent("attacking_card", 2, (X, Y) => { cbxPlayerAttack2.Card = Y.GetValueCard(); cbxPlayerAttack2.Refresh(); });
            core.GameState.AddStateChangedEvent("attacking_card", 3, (X, Y) => { cbxPlayerAttack3.Card = Y.GetValueCard(); cbxPlayerAttack3.Refresh(); });
            core.GameState.AddStateChangedEvent("attacking_card", 4, (X, Y) => { cbxPlayerAttack4.Card = Y.GetValueCard(); cbxPlayerAttack4.Refresh(); });
            core.GameState.AddStateChangedEvent("attacking_card", 5, (X, Y) => { cbxPlayerAttack5.Card = Y.GetValueCard(); cbxPlayerAttack5.Refresh(); });

            core.GameState.AddStateChangedEvent("defending_card", 0, (X, Y) => { cbxDefence.Card = Y.GetValueCard(); cbxDefence.Refresh(); });
            core.GameState.AddStateChangedEvent("defending_card", 1, (X, Y) => { cbxDefence1.Card = Y.GetValueCard(); cbxDefence.Refresh(); });
            core.GameState.AddStateChangedEvent("defending_card", 2, (X, Y) => { cbxDefence2.Card = Y.GetValueCard(); cbxDefence.Refresh(); });
            core.GameState.AddStateChangedEvent("defending_card", 3, (X, Y) => { cbxDefence3.Card = Y.GetValueCard(); cbxDefence.Refresh(); });
            core.GameState.AddStateChangedEvent("defending_card", 4, (X, Y) => { cbxDefence4.Card = Y.GetValueCard(); cbxDefence.Refresh(); });
            core.GameState.AddStateChangedEvent("defending_card", 5, (X, Y) => { cbxDefence5.Card = Y.GetValueCard(); cbxDefence.Refresh(); });
        }

        private void AddDeckEvents()
        {
            core.GameState.AddStateChangedEvent(Names.TRUMP_CARD, (X, Y) => cbxTrump.Card = Y.GetValueCard());
            core.GameState.AddStateChangedEvent(Names.TRUMP_CARD_USED, TrumpPickedUp);
            core.GameState.AddStateChangedEvent(Names.DECK_COUNT, (X, Y) =>
            {
                cplPlayersHand.UpdatePlayer();
                lblCardsLeft.Text = "" + Y.GetValueInt();
                if (Y.GetValueInt() == 0)
                    cbxDeck.Card = null;
            });
        }

        private void AddDiscardEvents()
        {
            core.GameState.AddStateChangedEvent(Names.DISCARD, (X, Y) =>
            {
                dscDiscard.Clear();
                foreach (var card in Y.GetValueCardCollection())
                    dscDiscard.AddCard(card);
            });
            core.GameState.AddStateChangedEvent(Names.TEMP_DISCARD, (X, Y) =>
            {
                dscTempDiscard.Clear();
                if (Y.RawValue != null)
                    foreach (var card in Y.GetValueCardCollection())
                        dscTempDiscard.AddCard(card);
            });
        }

        private void AddDefenderEvents()
        {
            core.GameState.AddStateChangedEvent(Names.DEFENDING_PLAYER, AttackingPlayersChanged);
            core.GameState.AddStateChangedEvent(Names.DEFENDER_FORFEIT, async (X, Y) =>
            {
                if (Y.GetValueBool())
                {
                    pbrDefenderCounter.Visible = true;
                    var progress = new Progress<int>(value => pbrDefenderCounter.Value = value) as IProgress<int>;
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            if (!core.GameState.GetValueBool(Names.DEFENDER_FORFEIT)) break;
                            if (progress != null) progress.Report(i);
                            Thread.Sleep(100);
                        }

                        if (InvokeRequired)
                            Invoke(new Action(() => pbrDefenderCounter.Visible = false));
                    });
                }
            });
        }

        private void AddRemainEvents()
        {
            core.GameState.AddStateChangedEvent(Names.PLAYER_FORFEIT, (X, Y) =>
            {
                var pair = playerUIs.FirstOrDefault(p => p.Key.ID == Y.GetValueInt());
                if (pair.Key != null && pair.Value.CardCount != null)
                    pair.Value.CardCount.ForeColor = SystemColors.ActiveCaptionText;

            });
            core.GameState.AddStateChangedEvent(Names.REMOVE_ALL_FORFEITS, (X, Y) =>
            {
                foreach (var tag in playerUIs.Values.Where(t => t.CardCount != null))
                    tag.CardCount.ForeColor = SystemColors.Control;
            });
            core.GameState.AddStateChangedEvent(Names.WINNING_PLAYERS, (X, Y) =>
            {
                var list = Y.RawValue as List<int>;
                if (list != null)
                {
                    foreach (var id in list)
                        foreach (var player in playerUIs.Keys.Where(p => p.ID == id))
                            playerUIs[player].Win.Visible = true;
                }
            });
            core.GameState.AddStateChangedEvent(Names.IS_GAME_OVER, GameOver);
        }

        public void AddSingleClientEvents(SingleClientServer singleServer)
        {
            this.singleServer = singleServer;
            singleServer.OnAddedCard += PlayerCardCountChanged;
            singleServer.OnRemovedCard += PlayerCardCountChanged;
        }

        public void AddMultiClientEvents(LobbyClient lobbyClient)
        {
            this.lobbyClient = lobbyClient;
            lobbyClient.OnPlayerChatGame += ReceivedChat;
            lobbyClient.OnPlayerCardCountChanged += PlayerCardCountChanged;
            lobbyClient.OnInvalidMove += InvalidMove;
            lobbyClient.OnPlayerKicked += DetermineKickButtons;
            lobbyClient.OnPlayerDigressed += PlayerDigressed;
            lobbyClient.OnPlayerDisconnectedGame += PlayerLeaved;
            lobbyClient.OnReturnToLobby += RemoveMultiClientEvents;
            lobbyClient.OnGameClose += Close;
        }

        private void RemoveDesignerEvents()
        {
            btnForfeit.Click -= new EventHandler(btnForfeit_Click);
            btnKickPlayer5.Click -= new EventHandler(KickPlayerPressed);
            btnKickPlayer1.Click -= new EventHandler(KickPlayerPressed);
            btnKickPlayer3.Click -= new EventHandler(KickPlayerPressed);
            btnKickPlayer2.Click -= new EventHandler(KickPlayerPressed);
            btnKickPlayer4.Click -= new EventHandler(KickPlayerPressed);
            btnMicrophone.Click -= new EventHandler(btnMicrophone_Click);
            btnSend.Click -= new EventHandler(btnSend_Click);
            cplPlayersHand.OnCardSelected -= new EventHandler<CardEventArgs>(cplPlayersHand_OnCardSelected);
            btnDigress.Click -= new EventHandler(btnDigress_Click);
            btnUnmute.Click -= new EventHandler(btnUnmute_Click);
            btnMute.Click -= new EventHandler(btnMute_Click);

            cplPlayersHand.Dispose(); //It took me a while to find this error. Without this, when closing the form, the memory is not cleared.
        }

        private void RemoveMultiClientEvents()
        {
            RemoveDesignerEvents();

            lobbyClient.OnPlayerChatGame -= ReceivedChat;
            lobbyClient.OnPlayerCardCountChanged -= PlayerCardCountChanged;
            lobbyClient.OnInvalidMove -= InvalidMove;
            lobbyClient.OnPlayerKicked -= DetermineKickButtons;
            lobbyClient.OnPlayerDigressed -= PlayerDigressed;
            lobbyClient.OnPlayerDisconnectedGame -= PlayerLeaved;
            lobbyClient.OnReturnToLobby -= RemoveMultiClientEvents;
            lobbyClient.OnGameClose -= Close;
            
            if (!lobbyClient.IsGameClosing)
                Close();
        }

        private void DetermineKickButtons()
        {
            foreach (var tag in playerUIs.Values)
            {
                if (tag.Kick != null)
                {
                    tag.Kick.Visible = false;
                    tag.Kick.Tag = null;
                    tag.Kick.Text = "";
                }
            }

            foreach (var player in core.ConnectedServer.Players.Values)
            {
                var btnKick = playerUIs[player].Kick;

                if (btnKick != null)
                {
                    if (!player.IsHost)
                        btnKick.Tag = player;

                    btnKick.Text = player.Name;
                    btnKick.Visible = true;
                }
            }
        }

        private void KickPlayerPressed(object sender, EventArgs e)
        {
            if (lobbyClient != null && lobbyClient.Player.IsHost)
            {
                var button = sender as Button;
                var player = button.Tag as Player;
                var tag = playerUIs.FirstOrDefault(p => p.Key.ID == player.ID).Value;

                if (tag.Leave.Visible)
                    MessageBox.Show("You have already kicked this player");
                else if (button != null && player != null)
                {
                    var result = MessageBox.Show(string.Format("Are you sure you want to kick {0}?", player.Name), "Confirm", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                        lobbyClient.RequestKick(player);
                }
            }
        }

        private void TrumpPickedUp(object sender, StateParameter p)
        {
            cbxTrump.Enabled = false;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMessage.Text))
                lobbyClient?.SendChatMessage(txtMessage.Text);

            txtMessage.Text = "";
        }

        private void ReceivedChat(Player player, string message)
        {
            var start = rtbChatLog.Text.Length;
            rtbChatLog.AppendText(player == null ? "[Server]" : "[" + player.Name + "]");
            rtbChatLog.Select(start, rtbChatLog.Text.Length - start);
            rtbChatLog.SelectionColor = player == null ? Color.Orange : player.IsHost ? Color.Yellow : Color.LightBlue;
            rtbChatLog.Select(rtbChatLog.Text.Length, 0);
            rtbChatLog.SelectionColor = Color.White;

            rtbChatLog.AppendText(" " + message + "\n");
            rtbChatLog.Select(rtbChatLog.Text.Length - 1, 1);
            rtbChatLog.ScrollToCaret();
        }

        private void GameOver(object sender, StateParameter p)
        {
            var message = "Game Over!\n";

            if (core.GameState.GetValueBool(Names.IS_TIE))
                message += "It's a tie!";
            else
            {
                Player durak = core.ConnectedServer.Players[core.GameState.GetValueInt(Names.LOSER_ID)];
                message += durak.Name + " is the Durak";
            }

            message += "\nPress OK to exit to lobby";
            MessageBox.Show(message, "Game over", MessageBoxButtons.OK);

            Close();
        }

        private void AttackingPlayersChanged(object sender, StateParameter p)
        {
            var attackingID = core.GameState.GetValueInt(Names.ATTACKING_PLAYER);
            var defendingID = core.GameState.GetValueInt(Names.DEFENDING_PLAYER);
            var attackingPlayer = core.ConnectedServer.Players[attackingID];
            var defendingPlayer = core.ConnectedServer.Players[defendingID];

            foreach (var pair in playerUIs)
            {
                if (pair.Value.Card != null)
                    pair.Value.Card.BackgroundImage =
                        pair.Key.ID == attackingID ? Properties.Resources.backAttacker
                      : pair.Key.ID == defendingID ? Properties.Resources.backDefender
                      : Properties.Resources.backThrower;
                else if (pair.Value.SwordShieldDagger != null)
                    pair.Value.SwordShieldDagger.BackgroundImage =
                        pair.Key.ID == attackingID ? Properties.Resources.statusAttacker
                      : pair.Key.ID == defendingID ? Properties.Resources.statusDefender
                      : Properties.Resources.statusThrower;

                if (pair.Value.Panel != null)
                    pair.Value.Panel.ShowBorder = false;
            }

            playerUIs[attackingPlayer].Panel.ShowBorder = true;
            playerUIs[attackingPlayer].Panel.BorderColor = Color.Red;
            playerUIs[defendingPlayer].Panel.ShowBorder = true;
            playerUIs[defendingPlayer].Panel.BorderColor = Color.Blue;
        }

        private void PlayerCardCountChanged(Player player, int newCardsCount, bool isCardCountChanged)
        {
            if (player.ID != core.Player.ID)
            {
                var tag = playerUIs[player];

                if (isCardCountChanged)
                {
                    tag.CardCount.Text = newCardsCount.ToString();
                    tag.CardCount.ForeColor = SystemColors.Control;
                }
                else
                    tag.CardCount.ForeColor = SystemColors.ActiveCaptionText;
            }
        }

        private void PlayerDigressed(int playerID, bool isDigress, bool isBot)
        {
            var tag = playerUIs.FirstOrDefault(p => p.Key.ID == playerID).Value;
            tag.Digress.Visible = isDigress;
            tag.BotGame.Visible = isBot;
        }

        private void PlayerLeaved(int playerID, bool isLeave, bool isBot)
        {
            var tag = playerUIs.FirstOrDefault(p => p.Key.ID == playerID).Value;
            tag.Leave.Visible = isLeave;
            tag.BotGame.Visible = isBot;
        }

        private void cplPlayersHand_OnCardSelected(object sender, CardEventArgs e)
        {
            var player = (sender as CardPlayer).Player;
            singleServer?.HandleMove(new GameMove(player, e.Card));

            if (!player.IsBot)
                lobbyClient?.RequestMove(e.Card);
            else
                MessageBox.Show("You cannot make a move while the bot is playing");
        }

        private void InvalidMove(Card card, string message)
        {
            MessageBox.Show(message, "Cannot play card");
        }

        private void btnForfeit_Click(object sender, EventArgs e)
        {
            singleServer?.HandleMove(new GameMove(core.Player, null));
            lobbyClient?.RequestMove(null);
        }

        private void btnDigress_Click(object sender, EventArgs e)
        {
            lobbyClient?.RequestDigress(!lobbyClient.Player.IsDigress, !lobbyClient.Player.IsBot);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (lobbyClient != null)
            {
                lobbyClient.IsGameClosing = true;

                if (lobbyClient.Player.IsHost)
                    lobbyClient.RequestServerState(ServerState.InLobby);
            }

            singleServer?.StopBots();
        }

        private void btnMicrophone_Click(object sender, EventArgs e)
        {
            isMicroActivated = !isMicroActivated;

            if (lobbyClient != null)
            {
                lobbyClient.Chat.MicroTurnOnOff(ref isMicroActivated);
                btnMicrophone.BackgroundImage = isMicroActivated ? Properties.Resources.microOn : Properties.Resources.microOff;
            }
        }

        private void btnUnmute_Click(object sender, EventArgs e)
        {
            if (lobbyClient != null)
            {
                foreach (var rbn in radioButtons.Where(b => b.Checked == true))
                {
                    if (rbn.Checked)
                    {
                        var pair = playerUIs
                            .Where(p => p.Key.ID != lobbyClient.Player.ID)
                            .Where(p => !p.Value.Leave.Visible)
                            .FirstOrDefault(p => p.Value.RadioButton == rbn);

                        if (pair.Key != null)
                        {
                            lobbyClient.Chat.MutedPlayers[pair.Key.IPAddress] = false;
                            rbn.Checked = false;
                            pair.Value.MutedPlayer.Visible = false;
                        }
                    }
                }
            }
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            if (lobbyClient != null)
            {
                foreach (var rbn in radioButtons.Where(b => b.Checked == true))
                {
                    var pair = playerUIs
                        .Where(p => p.Key.ID != lobbyClient.Player.ID)
                        .Where(p => !p.Value.Leave.Visible)
                        .FirstOrDefault(p => p.Value.RadioButton == rbn);

                    if (pair.Key != null)
                    {
                        lobbyClient.Chat.MutedPlayers[pair.Key.IPAddress] = true;
                        rbn.Checked = false;
                        pair.Value.MutedPlayer.Visible = true;
                    }
                }
            }
        }

        private void btnShowID_Click(object sender, EventArgs e)
        {
            foreach (var tag in playerUIs.Values)
                tag.PlayerID.Visible = !tag.PlayerID.Visible;
        }
    }
}
