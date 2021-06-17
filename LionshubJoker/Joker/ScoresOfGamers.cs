using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionshubJoker.Joker
{
    public class ScoresOfGamers
    {
        public List<Result> ScoresOfGamersForRound { get; set; }
        private readonly CardsOnRound _round;
        private readonly IList<Gamer> _gamers;
        public int waglejaODERshetenva
        {
            get => GetWaglejvaODERshetenva();
        }

        public ScoresOfGamers(CardsOnRound round, IList<Gamer> gamers)
        {
            _round = round;
            _gamers = gamers;
            ScoresOfGamersForRound = new List<Result>();
        }


        private int GetWaglejvaODERshetenva()
        {
            if (ScoresOfGamersForRound.Count == 4)
            {
                int sum = ScoresOfGamersForRound.Sum(p => Convert.ToInt32(p.ShouldScore));
                int maxScoreOfRound = Convert.ToInt32(_round);
                return maxScoreOfRound - sum;
            }
            return 0;
        }

        public void TellScore(Score score, int gamerId)
        {
            ScoresOfGamersForRound.Add(new Result
            {
                GamerId = gamerId,
                ShouldScore = score,
                IsScore = Score.Pass,
                MaxScore = (Score)Enum.ToObject(typeof(Score), Convert.ToInt16(_round))
            });
            Gamer gamer = _gamers.FirstOrDefault(p => p.Id == gamerId);
            gamer.Result.Add(new Result
            {
                GamerId = gamerId,
                ShouldScore = score,
                IsScore = Score.Pass,
                MaxScore = (Score)Enum.ToObject(typeof(Score), Convert.ToInt16(_round))
            });
        }

        public void AllowScoresForGamers(int gamerID)
        {
            Gamer gamer = _gamers.FirstOrDefault(p => p.Id == gamerID);
            if (gamer != null)
            {
                if (ScoresOfGamersForRound.Count != _gamers.Count - 1)
                {
                    gamer.AllowedScores.ForEach(p => p.Allowed = true);
                    int sumOfShouldScore = ScoresOfGamersForRound.Sum(p => (Convert.ToInt16(p.ShouldScore)));
                    if (sumOfShouldScore < gamer.AllowedScores.Count - 1)
                    {
                        gamer.ScoreToFill = (Score)Enum.ToObject(typeof(Score), (gamer.AllowedScores.Count - 1) - sumOfShouldScore);
                    }
                    else if (sumOfShouldScore == gamer.AllowedScores.Count - 1)
                    {
                        gamer.ScoreToFill = Score.Pass;
                    }
                    else
                    {
                        gamer.ScoreToFill = Score.None;
                    }
                }
                else
                {
                    int sumOfShouldScore = ScoresOfGamersForRound.Sum(p => (Convert.ToInt16(p.ShouldScore)));
                    if (sumOfShouldScore > gamer.AllowedScores.Count - 1)
                    {
                        gamer.AllowedScores.ForEach(p => p.Allowed = true);
                    }
                    else
                    {
                        gamer.AllowedScores.Where(p => p.Score != (Score)Enum.ToObject(typeof(Score), (gamer.AllowedScores.Count - 1) - sumOfShouldScore)).ToList().ForEach(p => p.Allowed = true);
                    }
                }
            }
        }
    }
}
