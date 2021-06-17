using System;
using System.Collections.Generic;
using System.Text;

namespace LionshubJoker.Joker
{
    public interface IDeckOfCardCreator
    {
        IList<Card> CreateDeckOfCards();
    }
}
