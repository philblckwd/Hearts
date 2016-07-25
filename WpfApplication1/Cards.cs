using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Cards
    {
        public cardValue value { get; private set; }
        public cardSuit suit { get; private set; }
        public List<string> cardsDealt = new List<string>();

        public enum cardValue
        {
            Ace = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13
        }

        public enum cardSuit
        {
            Clubs,
            Diamonds,
            Spades,
            Hearts
        }

        public bool generateCard()
        {
            bool dealt = false;
            Random rand = new Random();
            value = (cardValue)rand.Next(1, 14);
            suit = (cardSuit)rand.Next(0, 4);
            if (!(cardsDealt.Contains($"{value} of {suit}")))
            {
                cardsDealt.Add($"{value} of {suit}");
                dealt = true;
            }
            return dealt;
        }
    }
}
