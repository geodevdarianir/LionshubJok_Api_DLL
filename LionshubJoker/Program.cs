using System;
using System.Collections.Generic;
using System.Linq;
using LionshubJoker.Joker;

namespace LionshubJoker
{
    class Program
    {
        static void Main(string[] args)
        {
            //Game game = new Game(GameType.Standard);
            //game.LoadGame();
            Table table = new Table();
            // მოთამაშეების განსაზღვრა
            var gamers = new List<Gamer>
            {
                new Gamer (1,"Giorgi Labadze",table),
                new Gamer(2,"Rati Devdariani",table),
                new Gamer(3,"Giorgi Romanadze",table),
                new Gamer(4,"Anuki Devdariani",table)
            };


            // პირველი ხელი /// 1 კარტის დარიგება თითო მოთამაშეზე
            PlayGame play = new PlayGame(gamers);
            Game game = new Game(GameType.Standard, gamers);
            var rounds = game.LoadGame();
            //foreach (RoundsAndGamers round in rounds)
            //{
            RoundsAndGamers round = rounds.ElementAt(3);
            //Console.WriteLine("Round :{0} " + round);
            Console.WriteLine("");
            Console.WriteLine("Round ******* {0} ******", round.CurrentGamer);
            Console.WriteLine("Round ******* {0} *******", round.Hand);
            Console.WriteLine("");
            Console.WriteLine("");

            play.StartRound(round.Hand);
            play.CurrentGamer = round.CurrentGamer;

            if (play.TrumpCard == null)
            {
                return;
            }
            //var s= play.CurrentGamer.CardsOnHand;
            Console.WriteLine("");
            Console.WriteLine("TrumpCard ******* {0} ******", play.TrumpCard.ToString());
            Console.WriteLine("");
            ScoresOfGamers scores = new ScoresOfGamers(round.Hand, gamers);
            for (int i = 0; i < gamers.Count; i++)
            {
                foreach (Card card in play.CurrentGamer.CardsOnHand)
                {
                    Console.WriteLine("Gamer: {0} - {1}:{2}", play.CurrentGamer._name, play.CurrentGamer.CardsOnHand.IndexOf(card), card.ToString());
                }
                Console.WriteLine("");
                Console.WriteLine("SCORE ");
                Console.WriteLine("");
                scores.AllowScoresForGamers(play.CurrentGamer.Id);
                foreach (AllowedScores item in play.CurrentGamer.AllowedScores)
                {
                    Console.Write("{0} = {1} ", item.Score, item.Allowed);
                }
                Console.WriteLine("");
                int score = Convert.ToInt32(Console.ReadLine());
                Score TellScore = Enum.GetValues(typeof(Score)).Cast<Score>().ToList()[score];
                scores.TellScore(TellScore, play.CurrentGamer.Id);

                int indexOfGamer = gamers.IndexOf(play.CurrentGamer);
                if (indexOfGamer == gamers.Count - 1)
                {
                    play.CurrentGamer = gamers[0];
                }
                else
                {
                    play.CurrentGamer = gamers[indexOfGamer + 1];
                }
            }

            while (play.CurrentGamer.CardsOnHand.Count != 0)
            {
                while (table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count != gamers.Count)
                {
                    int indexOfGamer = gamers.IndexOf(play.CurrentGamer);
                    //int indexOfGamer = gamers.IndexOf(play.CurrentGamer);
                    play.CurrentGamer.AllowCardsForTable();
                    Console.WriteLine(play.Status);
                    foreach (Card card in play.CurrentGamer.CardsOnHand)
                    {
                        Console.WriteLine("Gamer: {0} - {1}:{2}", play.CurrentGamer._name, play.CurrentGamer.CardsOnHand.IndexOf(card), card.ToString());
                    }
                    int indexOfCard = Convert.ToInt32(Console.ReadLine());
                    if (indexOfCard > play.CurrentGamer.CardsOnHand.Count - 1)
                    {
                        continue;
                    }
                    if (play.CurrentGamer.CardsOnHand[indexOfCard].AllowsCardOnTheTable == false)
                    {
                        continue;
                    }
                    Console.WriteLine(play.CurrentGamer.CardsOnHand[indexOfCard].ToString());
                    if (table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count == 0 && play.CurrentGamer.CardsOnHand[indexOfCard].CardIsJoker() == true)
                    {
                        Console.WriteLine($"{play.CurrentGamer.Name} needs max Color : ");
                        Array colors = Enum.GetValues(typeof(CardColor));
                        foreach (var item in colors)
                        {
                            Console.WriteLine($"{Convert.ToInt32(item)}: {item.ToString()}");
                        }
                        int indexOfColor = Convert.ToInt32(Console.ReadLine());
                        Card tempCard = play.CurrentGamer.CardsOnHand[indexOfCard];
                        play.CurrentGamer.PutCardAway(play.CurrentGamer.CardsOnHand[indexOfCard]);
                        switch (indexOfColor)
                        {
                            case (int)CardColor.Spades:
                                table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.GiveAndTake = CardColor.Spades;
                                //play.CurrentGamer.CardsOnHand[indexOfCard].GiveAndTake = CardColor.Spades;
                                break;
                            case (int)CardColor.Hearts:
                                table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.GiveAndTake = CardColor.Hearts;
                                break;
                            case (int)CardColor.Diamonds:
                                table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.GiveAndTake = CardColor.Diamonds;
                                break;
                            case (int)CardColor.Clubs:
                                table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.GiveAndTake = CardColor.Clubs;
                                break;
                            case (int)CardColor.None:
                                table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.GiveAndTake = CardColor.None;
                                break;
                            default:
                                break;
                        }
                        if (indexOfGamer == gamers.Count - 1)
                        {
                            play.CurrentGamer = gamers[0];
                        }
                        else
                        {
                            play.CurrentGamer = gamers[indexOfGamer + 1];
                        }
                    }
                    else
                    {
                        play.CurrentGamer.PutCardAway(play.CurrentGamer.CardsOnHand[indexOfCard]);
                        if (indexOfGamer == gamers.Count - 1)
                        {
                            play.CurrentGamer = gamers[0];
                        }
                        else
                        {
                            play.CurrentGamer = gamers[indexOfGamer + 1];
                        }
                        //if (!(table._fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0].Card.CardIsJoker()))
                        //{

                        //}
                    }
                }
                play.CurrentGamer = gamers.Where(p => p.CurrentGamerAfterOneRound == true).FirstOrDefault();
                table.TakeCardsFromTable(round.Hand);
                play.CurrentGamer = gamers.Where(p => p.CurrentGamerAfterOneRound == true).First();
            }

            Console.WriteLine("");
            Console.WriteLine("Rerult after Round ******* {0} ******", round);
            Console.WriteLine("*****************************************************");
            foreach (Gamer item in gamers)
            {
                Console.WriteLine("{0} => {1}", item.Name, item.Result);
            }
            Console.WriteLine("******************************************************");
            Console.WriteLine("Next Round");
            Console.WriteLine("");
            //}


        }


    }
}
