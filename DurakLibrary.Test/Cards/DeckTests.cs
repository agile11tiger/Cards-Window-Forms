using DurakLibrary.Cards;
using NUnit.Framework;
using System.Linq;

namespace DurakLibrary.Test.Cards
{
    class DeckTests
    {
        private Deck deck;

        [SetUp]
        public void Setup()
        {
            deck = new Deck(CardValue.Seven, CardValue.Nine);
        }

        [Test]
        public void Draw_CardsCountEqualZero_NullReturned()
        {
            var cardColection = new CardCollection();
            var deck = new Deck(cardColection);
            Assert.IsNull(deck.Draw());
        }

        [Test]
        public void Draw_CardFaceUpReturned()
        {
            var card = deck.Draw();
            Assert.IsTrue(card.FaceUp);
        }

        [Test]
        public void AllCardsInDeckMustBeFaceDown()
        {
            foreach (var card in deck.Cards)
                Assert.IsFalse(card.FaceUp);
        }

    }
}