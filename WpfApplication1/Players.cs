using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Players
    {
        Cards card = new Cards();
        public string name { get; private set; }

        public Players(string name = "Default")
        {
            this.name = name;
        }

        public List<string> myCards = new List<string>();

        public void addCard(Cards.cardValue value, Cards.cardSuit suit)
        {
            myCards.Add($"{value} of {suit}");
        }

        public List<string> showCards()
        {
            return myCards;
        }
    }
}
