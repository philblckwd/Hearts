using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication1
{
    class Table
    {
        Cards cards = new Cards();
        public List<Players> players = new List<Players>();

        public Table()
        {
            
        }

        public void deal()
        {
            for (int i = 0; i < 13; i++)
            {
                foreach (Players player in players)
                {
                    bool dealt = cards.generateCard();
                    while (dealt == false)
                    {
                        dealt = cards.generateCard();
                    }
                    Cards.cardValue value = cards.value;
                    Cards.cardSuit suit = cards.suit;
                    player.addCard(value, suit);
                }
            }
        }

        public string findTwoOfClubs(List<Players> playersList)
        {
            bool found = false;
            int indexListbox = 0;
            int indexListboxItem = 0;
            for (int i = 0; i < playersList.Count; i++)
            {
                for (int j = 0; j < playersList[i].cardsListBox.Items.Count; j++)
                {
                    if (playersList[i].cardsListBox.Items[j].ToString() == "Two of Clubs")
                    {
                        found = true;
                        indexListbox = i;
                        indexListboxItem = j;
                        break;
                    }
                }
                if (found == true)
                {
                    break;
                }
            }
            return $"{indexListbox.ToString()}{indexListboxItem.ToString()}";
        }

        public int pickLowestCardOfCorrectSuit(string correctSuit, ListBox listbox)
        {
            List<string> validCardsValuesString = new List<string>();
            List<int> validCardsValuesInt = new List<int>();
            List<string> HeartsValuesString = new List<string>();
            List<int> HeartsValuesInt = new List<int>();

            for (int i = 0; i < listbox.Items.Count; i++)
            {
                string card = listbox.Items[i].ToString();
                if (card.Contains(correctSuit))
                {
                    string[] splitCorrect = card.Split();
                    validCardsValuesString.Add(splitCorrect[0]);
                }
                else if (card.Contains("Hearts"))
                {
                    string[] splitHearts = card.Split();
                    HeartsValuesString.Add(splitHearts[0]);
                }
            }
            convertArrayOfStringsToEnumValuesToIntegers(validCardsValuesString, validCardsValuesInt);
            convertArrayOfStringsToEnumValuesToIntegers(HeartsValuesString, HeartsValuesInt);

            if (validCardsValuesInt.Count > 0)
            {
                int index = getIndexOfMinValue(validCardsValuesInt);

                int indexListboxItem = 0;
                for (int j = 0; j < listbox.Items.Count; j++)
                {
                    if (listbox.Items[j].ToString() == $"{validCardsValuesString[index]} of {correctSuit}")
                    {
                        indexListboxItem = j;
                        break;
                    }
                }

                return indexListboxItem;
            }
            else
            {
                int index = getIndexOfMaxValue(HeartsValuesInt);

                int indexListboxItem = 0;
                for (int j = 0; j < listbox.Items.Count; j++)
                {
                    if (listbox.Items[j].ToString() == $"{validCardsValuesString[index]} of {correctSuit}")
                    {
                        indexListboxItem = j;
                        break;
                    }
                }

                return indexListboxItem;
            }
        }

        private int getIndexOfMinValue(List<int> validCardsValues)
        {
            return validCardsValues.IndexOf(validCardsValues.Min());
        }

        private int getIndexOfMaxValue(List<int> HeartsCardsValues)
        {
            return HeartsCardsValues.IndexOf(HeartsCardsValues.Max());
        }

        private void convertArrayOfStringsToEnumValuesToIntegers(List<string> stringCardValues, List<int> intCardValues)
        {
            foreach (string card in stringCardValues)
            {
                int value = (int)Enum.Parse(typeof(Cards.cardValue), card);
                intCardValues.Add(value);
            }
        }

        public string getStringSuitOfCard(string card)
        {
            string[] split = card.Split();
            return split[2];
        }

        public string getStringValueOfCard(string card)
        {
            string[] split = card.Split();
            return split[0];
        }

        public int getIntValueOfCard(string card)
        {
            string[] split = card.Split();
            int value = (int)Enum.Parse(typeof(Cards.cardValue), split[0]);
            return value;
        }

        public int returnIndexOfHighestCard(string startingCardSuit, string[] cards)
        {
            List<int> intValues = new List<int>();
            foreach(string card in cards)
            {
                if (getStringSuitOfCard(card) == startingCardSuit)
                {
                    intValues.Add(getIntValueOfCard(card));
                }
                else
                {
                    intValues.Add(0);
                }
            }
            int indexOfMax = intValues.IndexOf(intValues.Max());
            return indexOfMax;
        }

        public int returnScore(string[] cards)
        {
            int score = 0;
            foreach(string card in cards)
            {
                if (getStringSuitOfCard(card) == "Hearts")
                {
                    score += 1;
                }
                else if (card == "Queen of Spades")
                {
                    score += 13;
                }
            }
            return score;
        }
    }
}
