using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cards card = new Cards();
        Table table = new Table();
        Players player = new Players();

        List<Label> labels = new List<Label>();
        List<ListBox> listBoxes = new List<ListBox>();
        List<TextBlock> scoreBoxes = new List<TextBlock>();
        List<TextBlock> selectedCardLabels = new List<TextBlock>();
        Dictionary<ListBox, TextBlock> references = new Dictionary<ListBox, TextBlock>();

        int count = 0;
        int indexOfStartingPlayer;
        int indexOfLoser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Game_Loaded(object sender, RoutedEventArgs e)
        {
            addLabels();
            addListBoxes();
            addScoreBoxes();
            addSelectedCardLabels();
            addReferences();
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            makeObjectsVisible();

            table.addPlayers();
            table.deal();
            assignPlayers();
            dealCards();

            playBtn.Visibility = Visibility.Hidden;

            int indexListBox = showTwoOfClubs();

            indexOfStartingPlayer = indexListBox;

            if (selectedCardLabels[indexListBox] == references[listBox1])
            {
                onPickCardBtnClick(true);
            }
        }

        private void addLabels()
        {
            labels.Add(label1);
            labels.Add(label2);
            labels.Add(label3);
            labels.Add(label4);
        }

        private void addListBoxes()
        {
            listBoxes.Add(listBox1);
            listBoxes.Add(listBox2);
            listBoxes.Add(listBox3);
            listBoxes.Add(listBox4);
        }

        private void addScoreBoxes()
        {
            scoreBoxes.Add(player1Score);
            scoreBoxes.Add(Comp1Score);
            scoreBoxes.Add(Comp2Score);
            scoreBoxes.Add(Comp3Score);
        }

        private void addSelectedCardLabels()
        {
            selectedCardLabels.Add(selectedCardLabel1);
            selectedCardLabels.Add(selectedCardLabel2);
            selectedCardLabels.Add(selectedCardLabel3);
            selectedCardLabels.Add(selectedCardLabel4);
        }

        private void addReferences()
        {
            references.Add(listBox1, selectedCardLabel1);
            references.Add(listBox2, selectedCardLabel2);
            references.Add(listBox3, selectedCardLabel3);
            references.Add(listBox4, selectedCardLabel4);
        }

        private void assignPlayers()
        {
            int i = 0; // to cycle through players names for foreach loop below
            foreach (Label label in labels)
            {
                label.Content = table.players[i].name;
                i++;
            }
        }

        private void dealCards()
        {
            int i = 0; // again, to cycle through players names for foreach loop below
            foreach (ListBox listBox in listBoxes)
            {
                List<string> theCards = table.players[i].showCards();
                foreach (string card in theCards)
                {
                    listBox.Items.Add(card);
                }
                i++;
            }
        }

        private void onPickCardBtnClick(bool twoOfClubsPlayer1)
        {
            checkCountIterationToClearLabelsAfterRounds();
            string chosenCard;
            if (!twoOfClubsPlayer1)
            {
                chosenCard = listBoxes[0].SelectedItem.ToString();
                selectedCardLabels[0].Text = chosenCard;
                listBoxes[0].Items.Remove(listBoxes[0].SelectedItem);
            }
            else
            {
                chosenCard = "Two of Clubs";
            }

            for (int i = 1; i < listBoxes.Count; i++)
            {
                if (references[listBoxes[i]].Text == "")
                {
                    pickComputersCard(table.getStringSuitOfCard(chosenCard), listBoxes[i]);
                }
            }

            addScoreToLabelOfPlayerWithHighestValueCard();
        }

        private void pickCardBtn_Click(object sender, RoutedEventArgs e)
        {
            onPickCardBtnClick(false);
        }

        private void pickComputersCard(string suit, ListBox listbox)
        {
            int index = table.pickLowestCardOfCorrectSuit(suit, listbox);
            references[listbox].Text = listbox.Items[index].ToString();
            listbox.Items.Remove(listbox.Items[index].ToString());
        }

        private void addScoreToPlayer(int score, int index)
        {
            int receiver;
            if (int.TryParse(scoreBoxes[index].Text, out receiver))
            {
                receiver += score;
                scoreBoxes[index].Text = receiver.ToString();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void makeObjectsVisible()
        {
            listBoxes[0].Visibility = Visibility.Visible;
            pickCardBtn.Visibility = Visibility.Visible;
            foreach(TextBlock scorebox in scoreBoxes)
            {
                scorebox.Visibility = Visibility.Visible;
            }
        }

        private void checkCountIterationToClearLabelsAfterRounds()
        {
            if (count >= 1)
            {
                indexOfStartingPlayer = indexOfLoser;
                foreach (TextBlock label in selectedCardLabels)
                {
                    label.Text = "";
                }
            }
            count++;
        }
        
        private void addScoreToLabelOfPlayerWithHighestValueCard()
        {
            string startingPlayerSuit = table.getStringSuitOfCard(selectedCardLabels[indexOfStartingPlayer].Text);
            string[] selectedCardsForComparison = { selectedCardLabel1.Text, selectedCardLabel2.Text, selectedCardLabel3.Text, selectedCardLabel4.Text };
            indexOfLoser = table.returnIndexOfHighestCard(startingPlayerSuit, selectedCardsForComparison);
            int score = table.returnScore(selectedCardsForComparison);
            addScoreToPlayer(score, indexOfLoser);
            makeScoresGreen();
            scoreBoxes[indexOfLoser].Foreground = Brushes.Red;
        }

        private int showTwoOfClubs()
        {
            int indexListBox = int.Parse(table.findTwoOfClubs(listBoxes)[0].ToString());
            int indexListBoxItem = int.Parse(table.findTwoOfClubs(listBoxes)[1].ToString());

            selectedCardLabels[indexListBox].Text = listBoxes[indexListBox].Items[indexListBoxItem].ToString();
            listBoxes[indexListBox].Items.RemoveAt(indexListBoxItem);

            return indexListBox;
        }

        private void makeScoresGreen()
        {
            foreach (TextBlock score in scoreBoxes)
            {
                score.Foreground = Brushes.Green;
            }
        }

        private void checkForStartingPlayer(int indexOfLoser, string chosenCard)
        {
            if (indexOfLoser != 0)
            {
                if (indexOfLoser + 1 < listBoxes.Count)
                {
                    for (int i = indexOfLoser + 1; i < listBoxes.Count; i++)
                    {
                        pickComputersCard(table.getStringSuitOfCard(chosenCard), listBoxes[i]);
                    }
                }
            }
        }
    }
}
