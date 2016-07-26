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
        Table table = new Table();

        //List<TextBlock> labels = new List<TextBlock>();
        //List<ListBox> listBoxes = new List<ListBox>();
        //List<TextBlock> scoreBoxes = new List<TextBlock>();
        //List<TextBlock> selectedCardLabels = new List<TextBlock>();
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
            //addLabels();
            //addListBoxes();
            //addScoreBoxes();
            //addSelectedCardLabels();
            addReferences();
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            Players player1 = new Players(label1, listBox1, player1Score, selectedCardLabel1, "Player1");
            Players comp1 = new Players(label2, listBox2, Comp1Score, selectedCardLabel2, "Comp1");
            Players comp2 = new Players(label3, listBox3, Comp2Score, selectedCardLabel3, "Comp2");
            Players comp3 = new Players(label4, listBox4, Comp3Score, selectedCardLabel4, "Comp3");

            table.players.Add(player1);
            table.players.Add(comp1);
            table.players.Add(comp2);
            table.players.Add(comp3);

            makeObjectsVisible();
            
            table.deal();
            assignPlayers();
            dealCards();

            playBtn.Visibility = Visibility.Hidden;

            int indexListBox = showTwoOfClubs();

            indexOfStartingPlayer = indexListBox;

            if (table.players[indexListBox].selectedCardLabel == references[listBox1])
            {
                onPickCardBtnClick(true);
            }
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
            foreach (Players player in table.players)
            {
                player.nameLabel.Text = player.name;
            }
        }

        private void dealCards()
        {
            foreach(Players player in table.players)
            {
                List<string> theCards = player.showCards();
                foreach(string card in theCards)
                {
                    player.cardsListBox.Items.Add(card);
                }
            }
        }

        private void onPickCardBtnClick(bool twoOfClubsPlayer1)
        {
            checkCountIterationToClearLabelsAfterRounds();
            string chosenCard;
            if (!twoOfClubsPlayer1)
            {
                chosenCard = table.players[0].cardsListBox.SelectedItem.ToString();
                table.players[0].selectedCardLabel.Text = chosenCard;
                table.players[0].cardsListBox.Items.Remove(table.players[0].cardsListBox.SelectedItem);
            }
            else
            {
                chosenCard = "Two of Clubs";
            }

            for (int i = 1; i < table.players.Count; i++)
            {
                if (references[table.players[i].cardsListBox].Text == "")
                {
                    pickComputersCard(table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
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
            if (int.TryParse(table.players[index].scoreTextBlock.Text, out receiver))
            {
                receiver += score;
                table.players[index].scoreTextBlock.Text = receiver.ToString();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void makeObjectsVisible()
        {
            table.players[0].cardsListBox.Visibility = Visibility.Visible;
            pickCardBtn.Visibility = Visibility.Visible;
            foreach(Players player in table.players)
            {
                player.scoreTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void checkCountIterationToClearLabelsAfterRounds()
        {
            if (count >= 1)
            {
                indexOfStartingPlayer = indexOfLoser;
                foreach (Players player in table.players)
                {
                    player.selectedCardLabel.Text = "";
                }
            }
            count++;
        }
        
        private void addScoreToLabelOfPlayerWithHighestValueCard()
        {
            string startingPlayerSuit = table.getStringSuitOfCard(table.players[indexOfStartingPlayer].selectedCardLabel.Text);
            string[] selectedCardsForComparison = { selectedCardLabel1.Text, selectedCardLabel2.Text, selectedCardLabel3.Text, selectedCardLabel4.Text };
            indexOfLoser = table.returnIndexOfHighestCard(startingPlayerSuit, selectedCardsForComparison);
            int score = table.returnScore(selectedCardsForComparison);
            addScoreToPlayer(score, indexOfLoser);
            makeScoresGreen();
            table.players[indexOfLoser].scoreTextBlock.Foreground = Brushes.Red;
        }

        private int showTwoOfClubs()
        {
            int indexListBox = int.Parse(table.findTwoOfClubs(table.players)[0].ToString());
            int indexListBoxItem = int.Parse(table.findTwoOfClubs(table.players)[1].ToString());

            table.players[indexListBox].selectedCardLabel.Text = table.players[indexListBox].cardsListBox.Items[indexListBoxItem].ToString();
            table.players[indexListBox].cardsListBox.Items.RemoveAt(indexListBoxItem);

            return indexListBox;
        }

        private void makeScoresGreen()
        {
            foreach (Players player in table.players)
            {
                player.scoreTextBlock.Foreground = Brushes.Green;
            }
        }

        private void checkForStartingPlayer(int indexOfLoser, string chosenCard)
        {
            if (indexOfLoser != 0)
            {
                if (indexOfLoser + 1 < table.players.Count)
                {
                    for (int i = indexOfLoser + 1; i < table.players.Count; i++)
                    {
                        pickComputersCard(table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
                    }
                }
            }
        }
    }
}
