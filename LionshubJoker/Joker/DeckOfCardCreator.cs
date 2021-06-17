using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionshubJoker.Joker
{
    public class DeckOfCardCreator : IDeckOfCardCreator
    {
        /// <summary>
        /// ქმნის კარტის დასტას
        /// </summary>
        /// <returns>კარტის დასტა</returns>
        public IList<Card> CreateDeckOfCards()
        {

            var deckOfCards = new List<Card>();
            var allColorOfCard = Enum.GetValues(typeof(CardColor));
            var allValueOfCard = Enum.GetValues(typeof(CardValue));
            int index = 0;
            foreach (CardColor color in allColorOfCard)
            {
                if (color != CardColor.None)
                {
                    foreach (CardValue value in allValueOfCard)
                    {
                        if (value != CardValue.None)
                        {
                            index++;
                            Card card = new Card(color, value, index);
                            SetStrengthOfCard(card);
                            GeneratePathOfCards(card);
                            deckOfCards.Add(card);
                        }
                    }
                }
            }
            return deckOfCards;
        }
        private void GeneratePathOfCards(Card card)
        {
            switch (card.ColorOfCard)
            {
                case CardColor.Spades:
                    switch (card.ValueOfCard)
                    {
                        case CardValue.Ace:
                            card.CardPath = "/img/AS.png";
                            break;
                        case CardValue.Six:
                            card.CardPath = "/img/54.png";
                            break;
                        case CardValue.Seven:
                            card.CardPath = "/img/7S.png";
                            break;
                        case CardValue.Eight:
                            card.CardPath = "/img/8S.png";
                            break;
                        case CardValue.Nine:
                            card.CardPath = "/img/9S.png";
                            break;
                        case CardValue.Ten:
                            card.CardPath = "/img/10S.png";
                            break;
                        case CardValue.Jack:
                            card.CardPath = "/img/JS.png";
                            break;
                        case CardValue.Queen:
                            card.CardPath = "/img/QS.png";
                            break;
                        case CardValue.King:
                            card.CardPath = "/img/KS.png";
                            break;
                        default:
                            break;
                    }
                    break;
                case CardColor.Hearts:

                    switch (card.ValueOfCard)
                    {
                        case CardValue.Ace:
                            card.CardPath = "/img/AH.png";
                            break;
                        case CardValue.Six:
                            card.CardPath = "/img/6H.png";
                            break;
                        case CardValue.Seven:
                            card.CardPath = "/img/7H.png";
                            break;
                        case CardValue.Eight:
                            card.CardPath = "/img/8H.png";
                            break;
                        case CardValue.Nine:
                            card.CardPath = "/img/9H.png";
                            break;
                        case CardValue.Ten:
                            card.CardPath = "/img/10H.png";
                            break;
                        case CardValue.Jack:
                            card.CardPath = "/img/JH.png";
                            break;
                        case CardValue.Queen:
                            card.CardPath = "/img/QH.png";
                            break;
                        case CardValue.King:
                            card.CardPath = "/img/KH.png";
                            break;
                        default:
                            break;
                    }
                    break;
                case CardColor.Diamonds:

                    switch (card.ValueOfCard)
                    {
                        case CardValue.Ace:
                            card.CardPath = "/img/ADD.png";
                            break;
                        case CardValue.Six:
                            card.CardPath = "/img/6D.png";
                            break;
                        case CardValue.Seven:
                            card.CardPath = "/img/7D.png";
                            break;
                        case CardValue.Eight:
                            card.CardPath = "/img/8D.png";
                            break;
                        case CardValue.Nine:
                            card.CardPath = "/img/9D.png";
                            break;
                        case CardValue.Ten:
                            card.CardPath = "/img/10D.png";
                            break;
                        case CardValue.Jack:
                            card.CardPath = "/img/JD.png";
                            break;
                        case CardValue.Queen:
                            card.CardPath = "/img/QD.png";
                            break;
                        case CardValue.King:
                            card.CardPath = "/img/KD.png";
                            break;
                        default:
                            break;
                    }
                    break;
                case CardColor.Clubs:

                    switch (card.ValueOfCard)
                    {
                        case CardValue.Ace:
                            card.CardPath = "/img/AC.png";
                            break;
                        case CardValue.Six:
                            card.CardPath = "/img/53.png";
                            break;
                        case CardValue.Seven:
                            card.CardPath = "/img/7C.png";
                            break;
                        case CardValue.Eight:
                            card.CardPath = "/img/8C.png";
                            break;
                        case CardValue.Nine:
                            card.CardPath = "/img/9C.png";
                            break;
                        case CardValue.Ten:
                            card.CardPath = "/img/10C.png";
                            break;
                        case CardValue.Jack:
                            card.CardPath = "/img/JC.png";
                            break;
                        case CardValue.Queen:
                            card.CardPath = "/img/QC.png";
                            break;
                        case CardValue.King:
                            card.CardPath = "/img/KC.png";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        private void SetStrengthOfCard(Card card)
        {
            switch (card.ValueOfCard)
            {
                case CardValue.Six:
                    if (card.CardIsJoker())
                    {
                        card.Strength = StrengthOfCard.Joker;
                    }
                    else
                    {
                        card.Strength = StrengthOfCard.One;
                    }
                    break;
                case CardValue.Seven:
                    card.Strength = StrengthOfCard.Two;
                    break;
                case CardValue.Eight:
                    card.Strength = StrengthOfCard.Three;
                    break;
                case CardValue.Nine:
                    card.Strength = StrengthOfCard.Four;
                    break;
                case CardValue.Ten:
                    card.Strength = StrengthOfCard.Five;
                    break;
                case CardValue.Jack:
                    card.Strength = StrengthOfCard.Six;
                    break;
                case CardValue.Queen:
                    card.Strength = StrengthOfCard.Seven;
                    break;
                case CardValue.King:
                    card.Strength = StrengthOfCard.Eight;
                    break;
                case CardValue.Ace:
                    card.Strength = StrengthOfCard.Nine;
                    break;
                default:
                    break;
            }
        }
    }
}
