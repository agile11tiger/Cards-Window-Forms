using DurakLibrary.Cards;
using NUnit.Framework;

namespace DurakLibrary.Test.Cards
{
    public class CardTests
    {
        private Card AceHearts;
        private Card TenDiamonds;
        private Card TenClubs;
        private Card SevenHearts;
        private Card EightSpades;
        private Card QueenSpades;

        [SetUp]
        public void Setup()
        {
            AceHearts = new Card(CardValue.Ace, CardSuit.Hearts);
            TenDiamonds = new Card(CardValue.Ten, CardSuit.Diamonds);
            TenClubs = new Card(CardValue.Ten, CardSuit.Clubs);
            SevenHearts = new Card(CardValue.Seven, CardSuit.Hearts);
            EightSpades = new Card(CardValue.Eight, CardSuit.Spades);
            QueenSpades = new Card(CardValue.Queen, CardSuit.Spades);
        }

        #region Equals
        [Test]
        public void Equals_AceHeartsEqualsNull_ReturnFalse()
        {
            Assert.IsFalse(AceHearts.Equals(null));
        }

        [Test]
        public void Equals_AceHeartsEqualsAceHearts_ReturnTrue()
        {
            Assert.IsTrue(AceHearts.Equals(AceHearts));
        }

        [Test]
        public void Equals_AceHeartsEqualsSevenHearts_ReturnFalse()
        {
            Assert.IsFalse(AceHearts.Equals(SevenHearts));
        }

        [Test]
        public void Equals_AceHeartsEqualsTenDiamonds_ReturnFalse()
        {
            Assert.IsFalse(AceHearts.Equals(TenDiamonds));
        }
        #endregion
        #region Operator_Greater
        [Test]
        public void CompareNotTrumpCards_AceHeartsCreaterNull_ReturnTrue()
        {
            Assert.IsTrue(AceHearts > null);
        }

        [Test]
        public void CompareNotTrumpCards_NullCreaterSevenHearts_ReturnFalse()
        {
            Assert.IsFalse(null > SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsCreaterSevenHearts_ReturnTrue()
        {
            Assert.IsTrue(AceHearts > SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_SevenHeartsCreaterAceHearts_ReturnFalse()
        {
            Assert.IsFalse(SevenHearts > AceHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsCreaterTenDiamonds_ReturnFalse()
        {
            Assert.IsFalse(AceHearts > TenDiamonds);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsCreaterAceHearts_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds > AceHearts);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesCreaterEightSpades_ReturnTrue()
        {
            Assert.IsTrue(QueenSpades > EightSpades);
        }

        [Test]
        public void CompareTrumpCards_EightSpadesCreaterQueenSpades_ReturnFalse()
        {
            Assert.IsFalse(EightSpades > QueenSpades);
        }

        [Test]
        public void Compare_TrumpEightSpadesCreaterNotTrumpAceHearts_ReturnTrue()
        {
            Assert.IsTrue(EightSpades > AceHearts);
        }

        [Test]
        public void Compare_NotTrumpAceHeartsCreaterTrumpEightSpades_ReturnFalse()
        {
            Assert.IsFalse(AceHearts > EightSpades);
        }
        #endregion
        #region Operator_Less
        [Test]
        public void CompareNotTrumpCards_NullLessSevenHearts_ReturnTrue()
        {
            Assert.IsTrue(null < SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsLessNull_ReturnFalse()
        {
            Assert.IsFalse(AceHearts < null);
        }

        [Test]
        public void CompareNotTrumpCards_SevenHeartsLessAceHearts_ReturnTrue()
        {
            Assert.IsTrue(SevenHearts < AceHearts);
        }

        public void CompareNotTrumpCards_AceHeartsLessSevenHearts_ReturnFalse()
        {
            Assert.IsFalse(AceHearts < SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsLessAceHearts_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds < AceHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsLessTenDiamonds_ReturnFalse()
        {
            Assert.IsFalse(AceHearts < TenDiamonds);
        }

        [Test]
        public void CompareTrumpCards_EightSpadesLessQueenSpades_ReturnTrue()
        {
            Assert.IsTrue(EightSpades < QueenSpades);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesLessEightSpades_ReturnFalse()
        {
            Assert.IsFalse(QueenSpades < EightSpades);
        }

        [Test]
        public void Compare_NotTrumpAceHeartsLessTrumpEightSpades_ReturnTrue()
        {
            Assert.IsTrue(AceHearts < EightSpades);
        }

        [Test]
        public void Compare_TrumpEightSpadesLessNotTrumpAceHearts_ReturnFalse()
        {
            Assert.IsFalse(EightSpades < AceHearts);
        }
        #endregion
        #region Operator_GreaterOrEqual
        [Test]
        public void CompareNotTrumpCards_AceHeartsCreaterOrEqualNull_ReturnTrue()
        {
            Assert.IsTrue(AceHearts >= null);
        }

        [Test]
        public void CompareNotTrumpCards_NullCreaterOrEqualSevenHearts_ReturnFalse()
        {
            Assert.IsFalse(null >= SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsCreaterOrEqualSevenHearts_ReturnTrue()
        {
            Assert.IsTrue(AceHearts >= SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_SevenHeartsCreaterOrEqualAceHearts_ReturnFalse()
        {
            Assert.IsFalse(SevenHearts >= AceHearts);
        }

        [Test]
        public void CompareNotTrumpCards_SevenHeartsCreaterOrEqualSevenHearts_ReturnTrue()
        {
            Assert.IsTrue(SevenHearts >= SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsCreaterOrEqualTenDiamonds_ReturnFalse()
        {
            Assert.IsFalse(AceHearts >= TenDiamonds);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsCreaterOrEqualAceHearts_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds >= AceHearts);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsGreaterOrEqualTenClubs_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds >= TenClubs);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesCreaterOrEqualEightSpades_ReturnTrue()
        {
            Assert.IsTrue(QueenSpades >= EightSpades);
        }

        [Test]
        public void CompareTrumpCards_EightSpadesCreaterOrEqualQueenSpades_ReturnFalse()
        {
            Assert.IsFalse(EightSpades >= QueenSpades);
        }

        [Test]
        public void CompareTrumpCards_EightSpadesCreaterOrEqualEightSpades_ReturnTrue()
        {
            Assert.IsTrue(EightSpades >= EightSpades);
        }

        [Test]
        public void Compare_TrumpEightSpadesCreaterOrEqualNotTrumpAceHearts_ReturnTrue()
        {
            Assert.IsTrue(EightSpades >= AceHearts);
        }

        [Test]
        public void Compare_NotTrumpAceHeartsCreaterOrEqualTrumpEightSpades_ReturnFalse()
        {
            Assert.IsFalse(AceHearts >= EightSpades);
        }
        #endregion
        #region Operator_LessOrEqual
        [Test]
        public void CompareNotTrumpCards_NullLessOrEqualSevenHearts_ReturnTrue()
        {
            Assert.IsTrue(null < SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsLessOrEqualNull_ReturnFalse()
        {
            Assert.IsFalse(AceHearts < null);
        }

        [Test]
        public void CompareNotTrumpCards_SevenHeartsLessOrEqualAceHearts_ReturnTrue()
        {
            Assert.IsTrue(SevenHearts <= AceHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsLessOrEqualSevenHearts_ReturnFalse()
        {
            Assert.IsFalse(AceHearts <= SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsLessOrEqualAceHearts_ReturnTrue()
        {
            Assert.IsTrue(AceHearts <= AceHearts);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsLessOrEqualAceHearts_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds <= AceHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsLessOrEqualTenDiamonds_ReturnFalse()
        {
            Assert.IsFalse(AceHearts <= TenDiamonds);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsLessOrEqualTenClubs_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds <= TenClubs);
        }

        [Test]
        public void CompareTrumpCards_EightSpadesLessOrEqualQueenSpades_ReturnTrue()
        {
            Assert.IsTrue(EightSpades <= QueenSpades);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesLessOrEqualEightSpades_ReturnFalse()
        {
            Assert.IsFalse(QueenSpades <= EightSpades);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesLessOrEqualQueenSpades_ReturnTrue()
        {
            Assert.IsTrue(QueenSpades <= QueenSpades);
        }

        [Test]
        public void Compare_NotTrumpAceHeartsLessOrEqualTrumpEightSpades_ReturnTrue()
        {
            Assert.IsTrue(AceHearts <= EightSpades);
        }

        [Test]
        public void Compare_TrumpEightSpadesLessOrEqualNotTrumpAceHearts_ReturnFalse()
        {
            Assert.IsFalse(EightSpades <= AceHearts);
        }
        #endregion
        #region Operator_Equal
        [Test]
        public void CompareNotTrumpCards_NullEqualSevenHearts_ReturnFalse()
        {
            Assert.IsFalse(null == SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsEqualNull_ReturnFalse()
        {
            Assert.IsFalse(AceHearts == null);
        }

        [Test]
        public void CompareNotTrumpCards_TenClubsEqualTenClubs_ReturnTrue()
        {
            Assert.IsTrue(TenClubs == TenClubs);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsEqualSevenHearts_ReturnFalse()
        {
            Assert.IsFalse(AceHearts == SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsEqualTenClubs_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds == TenClubs);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsEqualAceHearts_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds == AceHearts);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesEqualQueenSpades_ReturnTrue()
        {
            Assert.IsTrue(QueenSpades == QueenSpades);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesEqualEightSpades_ReturnFalse()
        {
            Assert.IsFalse(QueenSpades == EightSpades);
        }
        #endregion
        #region Operator_UnEqual
        [Test]
        public void CompareNotTrumpCards_NullUnEqualSevenHearts_ReturnTrue()
        {
            Assert.IsTrue(null != SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsUnEqualNull_ReturnTrue()
        {
            Assert.IsTrue(AceHearts != null);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsUnEqualTenDiamonds_ReturnFalse()
        {
            Assert.IsFalse(TenDiamonds != TenDiamonds);
        }

        [Test]
        public void CompareNotTrumpCards_AceHeartsUnEqualSevenHearts_ReturnTrue()
        {
            Assert.IsTrue(AceHearts != SevenHearts);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsUnEqualTenClubs_ReturnTrue()
        {
            Assert.IsTrue(TenDiamonds != TenClubs);
        }

        [Test]
        public void CompareNotTrumpCards_TenDiamondsUnEqualAceHearts_ReturnTrue()
        {
            Assert.IsTrue(TenDiamonds != AceHearts);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesUnEqualQueenSpades_ReturnFalse()
        {
            Assert.IsFalse(QueenSpades != QueenSpades);
        }

        [Test]
        public void CompareTrumpCards_QueenSpadesUnEqualEightSpades_ReturnTrue()
        {
            Assert.IsTrue(QueenSpades != EightSpades);
        }
        #endregion
    }
}