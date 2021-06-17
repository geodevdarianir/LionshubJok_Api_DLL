using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LionshubJoker.Joker
{
    public class PlayGame
    {
        private readonly IList<Gamer> _gamers;
        private IList<Card> _deckOfCards;
        private CardsOnRound _cardsOnRound;
        private Card _trumpCard;

        public string Status { get { return CurrentGamer == null ? "Please start a round" : $"Waiting for player {CurrentGamer._name}"; } }
        public Gamer CurrentGamer { get; set; }
        public IList<Gamer> Gamers { get { return _gamers; } }
        public Card TrumpCard
        {
            get { return _trumpCard; }
            set
            {
                if (_cardsOnRound == CardsOnRound.Nine)
                    _trumpCard = value;
                else
                    GetTrumpCardOfRound();
            }
        }
        public PlayGame(IList<Gamer> gamer)
        {
            //_table = table;
            _gamers = gamer;
        }
        public void StartRound(CardsOnRound cardsOnHand)
        {
            _cardsOnRound = cardsOnHand;
            CreaDeckOfCard();
            EmptyHands();
            HandOutCardsToEachPlayer();
            GetTrumpCardOfRound();
            SetRoundScores(cardsOnHand);
        }

        private void CreaDeckOfCard()
        {
            // კარტის დასტის შექმნა
            DeckOfCardCreator deckOfCardCreator = new DeckOfCardCreator();
            // კარტის დასტა
            List<Card> deckOfCard = new List<Card>();
            deckOfCard.AddRange(deckOfCardCreator.CreateDeckOfCards());

            // კარტის დასტის აჩეხვა
            var mixDckOfCard = new MixDeckOfCard(deckOfCard);
            _deckOfCards = deckOfCard;
        }

        private void GetTrumpCardOfRound()
        {
            if (_cardsOnRound != CardsOnRound.Nine)
            {
                _trumpCard = _deckOfCards[0];
                if (_trumpCard.CardIsJoker() == false)
                {
                    AllCardIsTrump(_trumpCard.ColorOfCard);
                }
            }
        }

        public void SetTrumpCardOfRound(CardColor color)
        {
            if (_cardsOnRound == CardsOnRound.Nine)
            {
                _trumpCard = new Card(color, CardValue.None, 0);
                AllCardIsTrump(color);
            }
        }

        private void SetRoundScores(CardsOnRound round)
        {
            var allRounds = Enum.GetValues(typeof(CardsOnRound)).Cast<CardsOnRound>();
            var rounds = allRounds.Take(allRounds.ToList().IndexOf(round) + 1);
            foreach (Gamer item in _gamers)
            {
                item.AllowedScores.Clear();
                //item.AllowedScores.ForEach(p => p.Allowed = false);
                if (item.AllowedScores.Where(p => p.Score == Score.Pass).Count() == 0)
                {
                    item.AllowedScores.Add(new AllowedScores
                    {
                        Score = Score.Pass,
                        Allowed = false,
                    });
                }

                foreach (CardsOnRound roundItem in rounds)
                {
                    Score score = (Score)Enum.ToObject(typeof(Score), Convert.ToInt16(roundItem));
                    if (item.AllowedScores.Where(p => p.Score == score).Count() == 0)
                    {
                        item.AllowedScores.Add(new AllowedScores
                        {
                            Score = score,
                            Allowed = false,
                        });
                    }
                }
            }
        }

        private void AllCardIsTrump(CardColor color)
        {
            foreach (Gamer item in _gamers)
            {
                item.CardsOnHand.ForEach(p => p.IsTrump = false);
                item.CardsOnHand.Where(p => p.ColorOfCard == color).ToList().ForEach(p => p.IsTrump = true);
            }
        }

        private void HandOutCardsToEachPlayer()
        {
            foreach (Gamer item in _gamers)
            {
                for (int i = 0; i < Convert.ToInt16(_cardsOnRound); i++)
                {
                    item.CardsOnHand.Add(_deckOfCards[0]);
                    _deckOfCards.RemoveAt(0);
                }
                item.CardsOnHand = item.CardsOnHand.OrderByDescending(p => p.Strength).ThenBy(p => p.ColorOfCard).ToList();
            }
        }

        private void EmptyHands()
        {
            foreach (Gamer item in _gamers)
            {
                item.CardsOnHand.Clear();
            }
        }
    }
}
