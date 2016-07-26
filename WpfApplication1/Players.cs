using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication1
{
    class Players
    {
        Cards card = new Cards();
        public string name { get; private set; }
        public TextBlock nameLabel { get; private set; }
        public ListBox cardsListBox { get; private set; }
        public TextBlock scoreTextBlock { get; private set; }
        public TextBlock selectedCardLabel { get; private set; }

        public Players(TextBlock nameLabel, ListBox cardsListBox, TextBlock scoreTextBlocks, TextBlock selectedCardLabels, string name = "Default")
        {
            this.nameLabel = nameLabel;
            this.cardsListBox = cardsListBox;
            this.scoreTextBlock = scoreTextBlocks;
            this.selectedCardLabel = selectedCardLabels;
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
