using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionshubJoker.Joker
{
    public class Table
    {
        public class CardAndGamerOnTable
        {
            public int Index { get; set; }
            public Gamer Gamer { get; set; }
            public Card Card { get; set; }
        }
        public class FourCardsAndGamersOnTable
        {
            public List<CardAndGamerOnTable> _fourCardAndGamerOnTable { get; set; }
            public FourCardsAndGamersOnTable()
            {
                _fourCardAndGamerOnTable = new List<CardAndGamerOnTable>();
            }
        }
        //public readonly FourCardsAndGamersOnTable _fourCardsAndGamersListOnTheTable = new FourCardsAndGamersOnTable();
        public FourCardsAndGamersOnTable _fourCardsAndGamersListOnTheTable { get; private set; } = new FourCardsAndGamersOnTable();
        public Table()
        {
            _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable = new List<CardAndGamerOnTable>();
        }
        public void PlaceCardsOnTheTable(Card card, Gamer gamer)
        {

            _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Add(new CardAndGamerOnTable()
            {
                Card = card,
                Gamer = gamer,
            });
        }
        public void TakeCardsFromTable(CardsOnRound currentHand)
        {
            if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count != 0)
            {
                CardAndGamerOnTable firstCardAndGamer = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable[0];
                //variable cardsAndGamerOnTable არის იმდენი კარტი(მისი მფლობელი მოთამაში თურთ) რამდენსაც გვაძლევს ეს ფილტრი. ანუ პირველი კარტის ცვეტის ნაირი კარტები.
                List<CardAndGamerOnTable> cardsAndGamersOnTableOfSameColor = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.ColorOfCard == firstCardAndGamer.Card.ColorOfCard).ToList();
                // cardsAndGamersOnTable.Count == 0 ? _fourCardsAndGamersListOnTheTable._cardsOnTheTable[0].Card.Strength :

                if (firstCardAndGamer.Card.CardIsJoker() == false)
                {
                    StrengthOfCard strengthOfCardsAndGamersOnTable = cardsAndGamersOnTableOfSameColor.Where(p => p.Card.CardIsJoker() == false).Max(p => p.Card.Strength);
                    //პირველი კარტი არ არის ჯოკერი
                    if (cardsAndGamersOnTableOfSameColor.Count == _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker()).ToList().Count == 0)
                    {
                        // თუ მაგიდაზე ყველა ერთი ცვეტია და არ არის ჯოკერი, მაშინ წაიყვანოს ყველაზე მაღალმა ცვეტის მატარებელმა მოთამაშემ...
                        CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == strengthOfCardsAndGamersOnTable);
                        //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);

                        TakeCardGamer(winner.Gamer, currentHand);
                        _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                        winner.Gamer.CurrentGamerAfterOneRound = true;
                    }
                    else if (cardsAndGamersOnTableOfSameColor.Count != _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker()).ToList().Count == 0)
                    {
                        // როცა მაგიდაზე ერთი ცვეტის კარტი არ არის და არც ჯოკერია => მიყავს ამ ცვეტის მაღალ კარტს თუ კოზირი არაა მაგიდაზე.
                        List<CardAndGamerOnTable> trumpCardsOnTheTable = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump).ToList();
                        if (trumpCardsOnTheTable.Count == 0)
                        {
                            //მიყავს ამ ცვეტის მაღალ კარტს
                            CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == strengthOfCardsAndGamersOnTable);
                            //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                            TakeCardGamer(winner.Gamer, currentHand);
                            _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                            winner.Gamer.CurrentGamerAfterOneRound = true;
                        }
                        else
                        {
                            //მიყავს კოზირს
                            StrengthOfCard maxTrampCard = trumpCardsOnTheTable.Max(p => p.Card.Strength);
                            CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == maxTrampCard && p.Card.IsTrump);
                            //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                            TakeCardGamer(winner.Gamer, currentHand);
                            _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                            winner.Gamer.CurrentGamerAfterOneRound = true;
                        }
                    }
                    else if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker()).ToList().Count != 0)
                    {
                        //თუ მაგიდაზე ჯოკერი არის დადებული
                        List<CardAndGamerOnTable> jokersOnTable = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker()).ToList();
                        if (jokersOnTable.Count == 1)
                        {
                            //როცა ერთი ჯოკერი გვაქვს
                            CardAndGamerOnTable gamerWithJoker = jokersOnTable[0];
                            //როცა ჯოკერი არის ჯოკრის სიძლიერის
                            if (gamerWithJoker.Card.Strength == StrengthOfCard.Joker)
                            {
                                //gamerWithJoker.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                TakeCardGamer(gamerWithJoker.Gamer, currentHand);
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                gamerWithJoker.Gamer.CurrentGamerAfterOneRound = true;
                            }
                            else if (gamerWithJoker.Card.Strength == StrengthOfCard.LowJoker)
                            {
                                //როცა ჯოკერი არის შეცურებული
                                if (cardsAndGamersOnTableOfSameColor.Count == _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count)
                                {
                                    // თუ მაგიდაზე ყველა ერთი ცვეტია და არ არის ჯოკერი, მაშინ წაიყვანოს ყველაზე მაღალმა ცვეტის მატარებელმა მოთამაშემ
                                    CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == strengthOfCardsAndGamersOnTable);
                                    TakeCardGamer(winner.Gamer, currentHand);
                                    _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                    //winner.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    winner.Gamer.CurrentGamerAfterOneRound = true;
                                }
                                else
                                {
                                    // როცა მაგიდაზე ერთი ცვეტის კარტი არ არის და არც ჯოკერია => მიყავს ამ ცვეტის მაღალ კარტს თუ კოზირი არაა მაგიდაზე.
                                    List<CardAndGamerOnTable> trumpCardsOnTheTable = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump).ToList();
                                    if (trumpCardsOnTheTable.Count == 0)
                                    {
                                        CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == strengthOfCardsAndGamersOnTable);
                                        //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                        TakeCardGamer(winner.Gamer, currentHand);
                                        _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                        winner.Gamer.CurrentGamerAfterOneRound = true;
                                    }
                                    else
                                    {
                                        StrengthOfCard maxTrampCard = trumpCardsOnTheTable.Max(p => p.Card.Strength);
                                        CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == maxTrampCard);
                                        //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                        TakeCardGamer(winner.Gamer, currentHand);
                                        _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                        winner.Gamer.CurrentGamerAfterOneRound = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //როცა ორივე ჯოკერი დადებულია მაგიდაზე
                            if (jokersOnTable.Count > 1)
                            {
                                CardAndGamerOnTable jokerCard1 = jokersOnTable[0];
                                CardAndGamerOnTable jokerCard2 = jokersOnTable[0];
                                //როცა პირველი და მეორე ჯოკერიც ჯოკრის სიძლიერისაა
                                if (jokerCard1.Card.Strength == StrengthOfCard.Joker && jokerCard2.Card.Strength == StrengthOfCard.Joker)
                                {
                                    TakeCardGamer(jokerCard2.Gamer, currentHand);
                                    _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                    //jokerCard2.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    jokerCard2.Gamer.CurrentGamerAfterOneRound = true;
                                }
                                //როცა პირველი შეცურებულია და მეორე ჯოკერი ჯოკრავს
                                else if (jokerCard1.Card.Strength == StrengthOfCard.LowJoker && jokerCard2.Card.Strength == StrengthOfCard.Joker)
                                {
                                    TakeCardGamer(jokerCard2.Gamer, currentHand);
                                    //jokerCard2.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    jokerCard2.Gamer.CurrentGamerAfterOneRound = true;
                                }
                                //როცა პირველი ჯოკრავს და მეორე შეცურებული
                                else if (jokerCard1.Card.Strength == StrengthOfCard.Joker && jokerCard2.Card.Strength == StrengthOfCard.LowJoker)
                                {
                                    TakeCardGamer(jokerCard1.Gamer, currentHand);
                                    _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                    //jokerCard1.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    jokerCard1.Gamer.CurrentGamerAfterOneRound = true;
                                }
                                //როცა პირველი არის შეცურებული და მეორეც შეცურებული
                                else if (jokerCard1.Card.Strength == StrengthOfCard.LowJoker && jokerCard2.Card.Strength == StrengthOfCard.LowJoker)
                                {
                                    if (cardsAndGamersOnTableOfSameColor.Count == _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count)
                                    {
                                        // თუ მაგიდაზე ყველა ერთი ცვეტია და არ არის ჯოკერი, მაშინ წაიყვანოს ყველაზე მაღალმა ცვეტის მატარებელმა მოთამაშემ...
                                        CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == strengthOfCardsAndGamersOnTable);
                                        //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                        TakeCardGamer(winner.Gamer, currentHand);
                                        _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                        winner.Gamer.CurrentGamerAfterOneRound = true;
                                    }
                                    else
                                    {
                                        // როცა მაგიდაზე ერთი ცვეტის კარტი არ არის და არც ჯოკერია => მიყავს ამ ცვეტის მაღალ კარტს თუ კოზირი არაა მაგიდაზე.
                                        List<CardAndGamerOnTable> trumpCardsOnTheTable = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump).ToList();
                                        if (trumpCardsOnTheTable.Count == 0)
                                        {
                                            //თუ ყველა სხვადასხვა ცვეტისაა, პირველი რა ცვეტიც ჩამოვიდა ის წაიყვანს ანუ პირველი წაიყვანს იმიტო რო უნიკალურია
                                            CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == strengthOfCardsAndGamersOnTable);
                                            //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                            TakeCardGamer(winner.Gamer, currentHand);
                                            _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                            winner.Gamer.CurrentGamerAfterOneRound = true;
                                        }
                                        else
                                        {
                                            StrengthOfCard maxTrampCard = trumpCardsOnTheTable.Max(p => p.Card.Strength);
                                            CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.First(p => p.Card.Strength == maxTrampCard);
                                            //card.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                            TakeCardGamer(winner.Gamer, currentHand);
                                            _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                            winner.Gamer.CurrentGamerAfterOneRound = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //პირველი კარტია ჯოკერი
                    if (firstCardAndGamer.Card.Strength == StrengthOfCard.Joker)
                    {
                        //პირველი ჯოკერი ითხოვს ვიშს
                        if (firstCardAndGamer.Card.GiveAndTake != CardColor.None)
                        {
                            //GiveAndTake => ვსაზღვრავ პირველი ჯოკრის გარდა, ვინმე ჩამოვიდა თუ არა ჯოკერს მის შემდეგ. მეორის GiveAndTake= None
                            //List<CardAndGamerOnTable> cardsAndGamerOnTable = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.ColorOfCard == firstCardAndGamer.Card.GiveAndTake && p.Card.CardIsJoker() && p.Card.IsTrump).ToList();
                            if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count == 0 && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count == 0)
                            {
                                //ტრამპი არ არის ჩამოსული და მეორე ჯოკერი არ არის ჩამოსული
                                //firstCardAndGamer.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                TakeCardGamer(firstCardAndGamer.Gamer, currentHand);
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                firstCardAndGamer.Gamer.CurrentGamerAfterOneRound = true;
                            }
                            else if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count != 0 && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count == 0)
                            {
                                //კოზირი დარტყმულია და ჯოკერი არა
                                CardColor colorOfTrumCard = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).First().Card.ColorOfCard;
                                if (firstCardAndGamer.Card.GiveAndTake == colorOfTrumCard)
                                {
                                    //მიყავს ჯოკერს
                                    //firstCardAndGamer.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    TakeCardGamer(firstCardAndGamer.Gamer, currentHand);
                                    _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                    firstCardAndGamer.Gamer.CurrentGamerAfterOneRound = true;
                                }
                                else
                                {
                                    // მიყავს კოზირს
                                    StrengthOfCard trumpStrenght = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump).Max(p => p.Card.Strength);
                                    CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.Strength == trumpStrenght).First();
                                    //winner.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    TakeCardGamer(winner.Gamer, currentHand);
                                    _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                    firstCardAndGamer.Gamer.CurrentGamerAfterOneRound = true;
                                }

                            }
                            else if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count != 0 && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count != 0)
                            {
                                //კოზირი დარტყმულია და ჯოკერიც
                                CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).First();
                                //winner.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                TakeCardGamer(winner.Gamer, currentHand);
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                winner.Gamer.CurrentGamerAfterOneRound = true;
                            }
                            else if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count == 0 && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count != 0)
                            {
                                CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).First();
                                //winner.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                TakeCardGamer(winner.Gamer, currentHand);
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                winner.Gamer.CurrentGamerAfterOneRound = true;
                            }
                        }
                    }
                    else
                    {
                        // აცხადებს რომელ ცვეტს უნდა გაატანოს
                        if (firstCardAndGamer.Card.GiveAndTake != CardColor.None)
                        {
                            //List<CardAndGamerOnTable> cardsOnTheTable = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.ColorOfCard == firstCardAndGamer.Card.GiveAndTake && p.Card.CardIsJoker() && p.Card.IsTrump).ToList();
                            if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count == 0 && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count == 0)
                            {
                                //ტრამპი და ჯოკერი არ არის დარტყმული
                                if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.ColorOfCard == firstCardAndGamer.Card.GiveAndTake).Count() != 0)
                                {
                                    //firstCardAndGamer.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    TakeCardGamer(firstCardAndGamer.Gamer, currentHand);
                                    _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                    firstCardAndGamer.Gamer.CurrentGamerAfterOneRound = true;
                                }
                                else
                                {
                                    StrengthOfCard strengthOfCard = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.ColorOfCard == firstCardAndGamer.Card.GiveAndTake).Max(p => p.Card.Strength);
                                    CardAndGamerOnTable winnnerGamer = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.Strength == strengthOfCard).First();
                                    //winnnerGamer.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                    TakeCardGamer(winnnerGamer.Gamer, currentHand);
                                    _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                    winnnerGamer.Gamer.CurrentGamerAfterOneRound = true;
                                }
                            }
                            else if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count != 0 &&
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count == 0)
                            {
                                //კოზირი დარტყმულია და ჯოკერი არა
                                StrengthOfCard maxTrump = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Max(p => p.Card.Strength);
                                CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.Strength == maxTrump).First();
                                //winner.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                TakeCardGamer(winner.Gamer, currentHand);
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                winner.Gamer.CurrentGamerAfterOneRound = true;
                            }
                            else if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count != 0 && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count != 0)
                            {
                                //კოზირი დარტყმულია და ჯოკერიც
                                CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).First();
                                //winner.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                TakeCardGamer(winner.Gamer, currentHand);
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                winner.Gamer.CurrentGamerAfterOneRound = true;
                            }
                            else if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.IsTrump == true).ToList().Count == 0 && _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).ToList().Count != 0)
                            {
                                //კოზირი არ არის დარტყმულია და ჯოკერი დარტყმულია
                                CardAndGamerOnTable winner = _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Where(p => p.Card.CardIsJoker() == true && p.Card.GiveAndTake == CardColor.None).First();
                                TakeCardGamer(winner.Gamer, currentHand);
                                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.ForEach(p => p.Gamer.CurrentGamerAfterOneRound = false);
                                //winner.Gamer.TakenFourCardAndGamerFromTable.Add(_fourCardsAndGamersListOnTheTable);
                                winner.Gamer.CurrentGamerAfterOneRound = true;
                            }
                        }
                    }
                }
                _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Clear();
            }
        }
        private void TakeCardGamer(Gamer gamer, CardsOnRound hand)
        {
            foreach (CardAndGamerOnTable item in _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable)
            {
                gamer.TakenCardAndGamerFromTable.Add(new TakenCards
                {
                    Card = item.Card,
                    GamerId = item.Gamer.Id,
                    Hand = hand
                });
            }
            gamer.Result.First(p => p.Finished == false).IsScore = (Score)Enum.ToObject(typeof(Score), gamer.TakenCardAndGamerFromTable.Where(p => p.GamerId == gamer.Id && p.Hand == hand).Count());
            if (_fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable.Count() == 4)
            {
                foreach (CardAndGamerOnTable item in _fourCardsAndGamersListOnTheTable._fourCardAndGamerOnTable)
                {
                    if (item.Gamer.CardsOnHand.Count == 0)
                        item.Gamer.Result.First(p => p.Finished == false).Finished = true;
                }
            }
        }
    }
}