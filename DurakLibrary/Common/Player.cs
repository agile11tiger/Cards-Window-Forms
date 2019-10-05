using DurakLibrary.Cards;
using DurakLibrary.HostServer;
using System;
using System.Collections.Specialized;
using System.Net;

namespace DurakLibrary.Common
{
    public class Player : BotPlayer
    {
        public event EventHandler<Card> OnCardAddedToHand;
        public event EventHandler<Card> OnCardRemovedFromHand;
        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsHost { get; set; }
        public bool IsReady { get; set; }
        public bool IsBot { get; set; }
        public bool IsDigress { get; set; }
        public IPAddress IPAddress { get; set; }
        public CardCollection Hand { get; private set; }

        public Player(string name, int playerId = -1, bool isHost = false,
            bool isReady = false, bool isBot = false, bool isDigress = false)
        {
            Name = name;
            ID = playerId;
            IsHost = isHost;
            IsReady = isReady;
            IsBot = isBot;
            IsDigress = isDigress;
            Hand = new CardCollection();
        }

        public void AddEventCollectionChanged()
        {
            Hand.CollectionChanged += CardAddedOrRemoved;
        }

        private void CardAddedOrRemoved(object sender, NotifyCollectionChangedEventArgs e)
        {
            var list = e.NewItems ?? e.OldItems;
            var card = list?[0] as Card;

            if (e.Action == NotifyCollectionChangedAction.Add)
                OnCardAddedToHand?.Invoke(this, card);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                OnCardRemovedFromHand?.Invoke(this, card);
        }
    }
}
