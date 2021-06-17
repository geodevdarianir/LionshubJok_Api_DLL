using System;
using System.Collections.Generic;
using System.Text;

namespace LionshubJoker.Joker
{
    public  class MixDeckOfCard
    {
        // Shuffle a given array using Fisher–Yates shuffle Algorithm
        public  MixDeckOfCard(List<Card> DeckOfCard)
        {
            if (DeckOfCard == null) { throw new ArgumentException("DeckOfCard"); }
            var random = new Random();
            for (int i = DeckOfCard.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(0, i + 1);
                var card = DeckOfCard[randomIndex];
                DeckOfCard.RemoveAt(randomIndex);
                DeckOfCard.Add(card);
            }
        }
    }
}
