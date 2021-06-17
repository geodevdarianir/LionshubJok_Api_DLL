using System;
using System.Collections.Generic;
using System.Text;

namespace LionshubJoker.Joker
{
    public class Card : IEquatable<Card>
    {

        private readonly CardColor _cardColor;
        private readonly CardValue _cardValue;
        private readonly int _cardId;

        public CardColor ColorOfCard { get { return _cardColor; } }
        public CardValue ValueOfCard { get { return _cardValue; } }
        public int CardId { get { return _cardId; } }
        public string CardPath { get; set; }
        public StrengthOfCard Strength { get; set; }
        public CardColor GiveAndTake { get; set; }
        public bool IsTrump { get; set; }
        public bool AllowsCardOnTheTable { get; set; }
        public CardColor JokerNeedsCard { get; set; }

        public Card(CardColor color, CardValue value, int id)
        {
            _cardColor = color;
            _cardValue = value;
            _cardId = id;
            GiveAndTake = CardColor.None;
        }


        public bool CardIsJoker()
        {
            if ((_cardValue == CardValue.Six && _cardColor == CardColor.Clubs) || (_cardValue == CardValue.Six && _cardColor == CardColor.Spades))
                return true;
            return false;
        }

        public void SetJokerStrength(StrengthOfCard strengthOfCard, CardColor giveANDtake)
        {
            if (this.CardIsJoker())
            {
                if (strengthOfCard == StrengthOfCard.LowJoker)
                {
                    this.Strength = StrengthOfCard.LowJoker;
                }
                else
                {
                    this.Strength = StrengthOfCard.Joker;
                }
                GiveAndTake = giveANDtake;
            }
        }



        public bool Equals(Card otherCard)
        {
            if (ReferenceEquals(otherCard, null))
            {
                return false;
            }
            return _cardColor == otherCard._cardColor && _cardValue == otherCard._cardValue;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as Card);
        }

        public override int GetHashCode()
        {
            return _cardValue.GetHashCode() ^ _cardColor.GetHashCode();
        }

        public override string ToString()
        {
            string status = AllowsCardOnTheTable == true ? "A" : "N";
            if (CardIsJoker())
            {
                return $"{status}: {_cardId} => Joker";
            }
            else
            {
                return $"{status}: {_cardId} => {_cardValue.ToString()} - {_cardColor.ToString()}";
            }
        }

        public static bool operator ==(Card firstCard, Card secondCard)
        {
            if (ReferenceEquals(null, firstCard))
            {
                return ReferenceEquals(null, secondCard);
            }
            return firstCard.Equals(secondCard);
        }

        public static bool operator !=(Card firstCard, Card secondCard)
        {
            return !(firstCard == secondCard);
        }
    }
}
