using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire
{
    public static class FisherYates
    {
        static Random r = new Random();

        public static List<Card> Shuffle(List<Card> deck)
        {
            for (int n = deck.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                int tempsuit = deck[n].suit;
                int temp = deck[n].rank;
                string card = deck[n].card;

                deck[n].rank = deck[k].rank;
                deck[k].rank = temp;
                deck[n].suit = deck[k].suit;
                deck[k].suit = tempsuit;
                deck[n].card = deck[k].card;
                deck[k].card = card;
            }

            return deck;
        }
    }
}