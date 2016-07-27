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
        Random rand = new Random();
        Cards card = new Cards();
        
        int indexOfStartingPlayer;
        int indexOfLoser;
        string chosenCard;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Game_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            Players player1 = new Players(label1, listBox1, player1Score, selectedCardLabel1, "Player1"); //simplify with struct
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

            int indexListBox = getTwoOfClubs();

            indexOfStartingPlayer = indexListBox;

            if (table.players[indexOfStartingPlayer].selectedCardLabel == table.players[0].selectedCardLabel)
            {
                onPickCardBtnClick(true);
            }
            else
            {
                checkForStartingPlayer();
            }
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
        
        private void onPickCardBtnClick(bool twoOfClubs)
        {
            if (!twoOfClubs)
            {
                if (indexOfStartingPlayer == 0)
                {
                    chosenCard = table.players[0].cardsListBox.SelectedItem.ToString();
                    table.players[0].selectedCardLabel.Text = chosenCard;
                    table.players[0].cardsListBox.Items.Remove(table.players[0].cardsListBox.SelectedItem);
                    for (int i = 1; i < table.players.Count; i++)
                    {
                        if (table.players[i].selectedCardLabel.Text == "")
                        {
                            pickComputersCard(false, table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
                        }
                    }
                }
                else
                {
                    table.players[0].selectedCardLabel.Text = table.players[0].cardsListBox.SelectedItem.ToString();
                    table.players[0].cardsListBox.Items.Remove(table.players[0].cardsListBox.SelectedItem);
                    for (int i=1;i<table.players.Count; i++)
                    {
                        if (table.players[i].selectedCardLabel.Text == "")
                        {
                            pickComputersCard(false, table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
                        }
                    }
                }
            }
            else
            {
                chosenCard = "Two of Clubs";
                for(int i=1; i<table.players.Count; i++)
                {
                    pickComputersCard(false, table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
                }
                pickCardBtn.Visibility = Visibility.Hidden;
                nextRoundBtn.Visibility = Visibility.Visible;
            }

            //pickComputersCard(((Cards.cardSuit)rand.Next(0, 4)).ToString(), table.players[indexOfStartingPlayer].cardsListBox);
            //chosenCard = table.players[indexOfStartingPlayer].selectedCardLabel.Text;
            //checkForStartingPlayer(indexOfStartingPlayer, chosenCard);

            //for (int i = 1; i < table.players.Count; i++)
            //{
            //    if (references[table.players[i].cardsListBox].Text == "")
            //    {
            //        pickComputersCard(table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
            //    }
            //}

            addScoreToLabelOfPlayerWithHighestValueCard();
            indexOfStartingPlayer = indexOfLoser;
        }

        private void pickCardBtn_Click(object sender, RoutedEventArgs e)
        {
            bool areLabelsEmpty = checkIfLabelsAreEmpty();
            if (areLabelsEmpty)
            {
                onPickCardBtnClick(false);
            }
            else
            {
                checkForStartingPlayer();
            }
            pickCardBtn.Visibility = Visibility.Hidden;
            nextRoundBtn.Visibility = Visibility.Visible;
        }

        private void nextRound_Click(object sender, RoutedEventArgs e)
        {
            clearAllLabels();
            if (indexOfLoser != 0)
            {
                checkForStartingPlayer();
            }
            nextRoundBtn.Visibility = Visibility.Hidden;
            pickCardBtn.Visibility = Visibility.Visible;
        }

        private void pickComputersCard(bool isStartingCard, string suit, ListBox listbox)
        {
            TextBlock selectedLabel = new TextBlock();
            foreach (Players player in table.players)
            {
                if (player.cardsListBox == listbox)
                {
                    selectedLabel = player.selectedCardLabel;
                }
            }

            int index = table.pickLowestCardOfCorrectSuit(suit, listbox);
            selectedLabel.Text = listbox.Items[index].ToString();
            listbox.Items.Remove(listbox.Items[index].ToString());
            if (isStartingCard)
            {
                chosenCard = selectedLabel.Text;
            }
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

        //private void checkCountIterationToClearLabelsAfterRounds()
        //{
        //    if (count >= 1)
        //    {
        //        indexOfStartingPlayer = indexOfLoser;
        //        foreach (Players player in table.players)
        //        {
        //            player.selectedCardLabel.Text = "";
        //        }
        //    }
        //    count++;
        //}

        private bool checkIfLabelsAreEmpty()
        {
            int count = 0;
            for(int i=1; i<table.players.Count; i++)
            {
                if (table.players[i].selectedCardLabel.Text == "")
                {
                    count++;
                }
            }
            bool x = count > 0 ? true : false;
            return x;
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

        private int getTwoOfClubs()
        {
            int indexListBox = int.Parse(table.findTwoOfClubs(table.players)[0].ToString());
            int indexListBoxItem = int.Parse(table.findTwoOfClubs(table.players)[1].ToString());

            table.players[indexListBox].selectedCardLabel.Text = table.players[indexListBox].cardsListBox.Items[indexListBoxItem].ToString();
            table.players[indexListBox].cardsListBox.Items.RemoveAt(indexListBoxItem);

            chosenCard = table.players[indexListBox].selectedCardLabel.Text;

            return indexListBox;
        }

        private void makeScoresGreen()
        {
            foreach (Players player in table.players)
            {
                player.scoreTextBlock.Foreground = Brushes.Green;
            }
        }

        //private void checkForStartingPlayer(int indexOfLoser, string chosenCard)
        //{
        //    isClicked = false;
        //    if (indexOfLoser != 0)
        //    {
        //        if (indexOfLoser + 1 < table.players.Count)
        //        {
        //            for (int i = indexOfLoser + 1; i < table.players.Count; i++)
        //            {
        //                pickComputersCard(table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
        //            }
        //        }
        //        if (isClicked)
        //        {
        //            for (int i = 1; i < table.players.Count; i++)
        //            {
        //                if (references[table.players[i].cardsListBox].Text == "")
        //                {
        //                    pickComputersCard(table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
        //                }
        //            }
        //        }
        //    }
        //}

        private void checkForStartingPlayer()
        {
            if (indexOfStartingPlayer != 0)
            {
                pickComputersCard(true, ((Cards.cardSuit)rand.Next(0, 4)).ToString(), table.players[indexOfStartingPlayer].cardsListBox);
                for (int i = indexOfStartingPlayer + 1; i < table.players.Count; i++)
                {
                    pickComputersCard(false, table.getStringSuitOfCard(chosenCard), table.players[i].cardsListBox);
                }
            }
        }

        private void clearAllLabels()
        {
            foreach(Players player in table.players)
            {
                player.selectedCardLabel.Text = "";
            }
        }
    }
}
