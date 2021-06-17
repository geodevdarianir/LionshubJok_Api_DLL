using System;
using System.Collections.Generic;
using System.Text;

namespace LionshubJoker.Joker
{
    public class TakenCards
    {
        public Card Card { get; set; }
        public int GamerId { get; set; }
        public CardsOnRound Hand { get; set; }
    }
}
