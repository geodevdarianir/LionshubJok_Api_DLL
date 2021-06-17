using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace LionshubJoker.Joker
{
    public class Gamer
    {
        private readonly Table _table;
        public readonly int _id;
        public readonly string _name;
        public List<Card> _cardsOnHand = new List<Card>();
        public string Name { get { return _name; } }
        public int Id { get { return _id; } }
        public List<Card> CardsOnHand
        {
            get
            {
                //_cardsOnHand = _cardsOnHand.OrderByDescending(p => p.ColorOfCard).OrderByDescending(p => p.Strength).ToList();
                return _cardsOnHand;
                /*_cardsOnHand.OrderByDescending(p => p.ColorOfCard).OrderByDescending(p => p.Strength).ToList();*/
            }
            set
            {
                _cardsOnHand = value;
            }
        }
        public List<Result> Result { get; set; }
        public List<AllowedScores> AllowedScores { get; set; }
        public Score ScoreToFill { get; set; }
        public bool CurrentGamerAfterOneRound { get; set; }
        public List<TakenCards> TakenCardAndGamerFromTable { get; set; }

        public bool IsOnPremia { get => GamerOnPremia(); }
        public Gamer(int id, string name, Table table)
        {
            _id = id;
            _name = name;
            _table = table;
            TakenCardAndGamerFromTable = new List<TakenCards>();
            AllowedScores = new List<AllowedScores>();
            Result = new List<Result>();
        }

        private bool GamerOnPremia()
        {
            foreach (Result item in Result)
            {
                if (item.IsScore != item.ShouldScore)
                {
                    return false;
                }
            }
            return true;
        }

        public void SetShouldScore(Score ShouldScore, RoundsAndGamers roundGamerANDpulka)
        {
            Result.Add(new Result()
            {
                ShouldScore = ShouldScore,
                MaxScore = (Score)Enum.ToObject(typeof(Score), Convert.ToInt16(roundGamerANDpulka.Hand)),
                IsScore = Score.Pass,
                GamerId = this.Id,
                Hand = roundGamerANDpulka
            });
        }

        public bool PutCardAway(Card card)
        {
            bool result = false;
            if (card.AllowsCardOnTheTable)
            {
                _cardsOnHand.Remove(card);
                _table.PlaceCardsOnTheTable(card, this);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// ამოწმებს მოთამაშეს აქვს თუ არა ე.წ. ცვეტი ხელში
        /// </summary>
        /// <param name="trumpCard">ე.წ. ცვეტი </param>
        /// <returns></returns>
        public bool ContainsColorOfCardOnHand(CardColor musterCard)
        {
            foreach (Card card in _cardsOnHand)
            {
                if (card.ColorOfCard == musterCard)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ააქტიურებს შესაბამის მოთამაშის კარტებს. კოზირი, ცვეტი. ჯოკერი ყოველთვის აქტიურია
        /// </summary>
        /// <param name="trumpCard">კოზირი</param>
        public void AllowCardsForTable()
        {
            CardColor colorOfCard = CardColor.None;
            //_table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count == 0 ? CardColor.None : _table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.ColorOfCard;
            if (_table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count != 0)
            {
                if (_table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.CardIsJoker() == true)
                {
                    colorOfCard = _table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.GiveAndTake;
                }
                else
                {
                    colorOfCard = _table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.ColorOfCard;
                }
            }
            if (_table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count == 0)
            {
                _cardsOnHand.ForEach(s => s.AllowsCardOnTheTable = true);
            }
            else
            {
                _cardsOnHand.ForEach(s => s.AllowsCardOnTheTable = false);

                if (_cardsOnHand.Where(p => p.ColorOfCard == colorOfCard).ToList().Count != 0)
                {
                    _cardsOnHand.Where(p => p.ColorOfCard == colorOfCard).ToList().ForEach(s => s.AllowsCardOnTheTable = true);
                }
                else
                {
                    if (_cardsOnHand.Where(p => p.IsTrump == true).ToList().Count != 0)
                    {
                        _cardsOnHand.Where(p => p.IsTrump == true).ToList().ForEach(s => s.AllowsCardOnTheTable = true);
                    }
                    else
                    {
                        _cardsOnHand.ForEach(s => s.AllowsCardOnTheTable = true);
                    }
                }
                if (_cardsOnHand.Where(p => p.CardIsJoker() == true).ToList().Count != 0)
                {
                    _cardsOnHand.Where(p => p.CardIsJoker() == true).ToList().ForEach(s => s.AllowsCardOnTheTable = true);
                }
            }
        }
        /// <summary>
        /// ააქტიურებს ჯოკერს
        /// </summary>
        private void AllowJoker()
        {
            foreach (Card item in _cardsOnHand)
            {
                if (item.CardIsJoker())
                {
                    item.AllowsCardOnTheTable = true;
                }
            }
        }

        /// <summary>
        /// ცვეტს ააქტიურებს
        /// </summary>
        /// <param name="colorOfCard">კარტის ფერი</param>
        private void AllowMusterCards(CardColor colorOfCard)
        {
            foreach (Card card in CardsOnHand)
            {
                if (card.ColorOfCard == colorOfCard)
                {
                    card.AllowsCardOnTheTable = true;
                }
                else
                {
                    card.AllowsCardOnTheTable = false;
                }
            }
        }

        /// <summary>
        /// ააქტიურებს ჯოკრის მიერ მოთხოვნილ მაქსიმალურ კარტს. (ვიში ჯვარი, ყვავი, აგური, გული)
        /// </summary>
        /// <param name="cardColorOfMaxCard">მაღალი "ცვეტი"</param>
        public void AllowMaxCardsForTable(CardColor cardColorOfMaxCard)
        {
            if (_cardsOnHand.Where(p => p.ColorOfCard == cardColorOfMaxCard).ToList().Count != 0)
            {
                StrengthOfCard maxStr = _cardsOnHand.Where(p => p.ColorOfCard == cardColorOfMaxCard).Max(p => p.Strength);
                _cardsOnHand.Where(p => p.ColorOfCard == cardColorOfMaxCard && p.Strength == maxStr).First().AllowsCardOnTheTable = true;
            }
            else if (_cardsOnHand.Where(p => p.IsTrump == true).ToList().Count != 0)
            {
                _cardsOnHand.Where(p => p.IsTrump == true).ToList().ForEach(p => p.AllowsCardOnTheTable = true);
            }
            else
            {
                _cardsOnHand.ForEach(p => p.AllowsCardOnTheTable = true);
            }
            if (_cardsOnHand.Where(p => p.CardIsJoker() == true).ToList().Count != 0)
            {
                _cardsOnHand.Where(p => p.CardIsJoker() == true).ToList().ForEach(p => p.AllowsCardOnTheTable = true);
            }
            //if (ContainsColorOfCardOnHand(cardColorOfMaxCard))
            //{
            //    AllowMaxCards(cardColorOfMaxCard, false);
            //    AllowJoker();
            //}
            //else if (ContainsColorOfCardOnHand(trumpCard.ColorOfCard))
            //{
            //    AllowMaxCards(trumpCard.ColorOfCard, true);
            //    AllowJoker();
            //}
            //else
            //{
            //    foreach (Card item in _cardsOnHand)
            //    {
            //        item.AllowsCardOnTheTable = true;
            //    }
            //}
        }
        readonly List<Card> cardValues = new List<Card>();
        private void AllowMaxCards(CardColor colorOfCard, bool trump)
        {
            if (trump)
            {
                foreach (var item in CardsOnHand)
                {
                    if (item.ColorOfCard == colorOfCard)
                    {
                        item.AllowsCardOnTheTable = true;
                    }
                    else
                    {
                        item.AllowsCardOnTheTable = false;
                    }
                }
            }
            else
            {
                StrengthOfCard strength = default;
                int index = 0;
                foreach (Card card in CardsOnHand)
                {
                    index++;

                    if (card.ColorOfCard == colorOfCard && !card.CardIsJoker())
                    {
                        cardValues.Add(card);
                        strength = cardValues.Max(p => p.Strength);
                    }

                    if (CardsOnHand.Count == index)
                    {
                        foreach (var cardo in CardsOnHand)
                        {
                            if (cardo.ColorOfCard == colorOfCard)
                            {
                                if (cardo.Strength == strength)
                                {
                                    cardo.AllowsCardOnTheTable = true;
                                }
                                else
                                {
                                    cardo.AllowsCardOnTheTable = false;
                                }
                            }
                            else
                            {
                                //jokers zevit mainc trued vxdi!
                                cardo.AllowsCardOnTheTable = false;
                            }

                        }
                    }
                }
            }

        }

    }
}
