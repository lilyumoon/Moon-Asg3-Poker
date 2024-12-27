using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_Asg3_Poker
{
    public class Card
    {
        public enum SuitType { Clubs, Diamonds, Hearts, Spades }

        private int rank;
        private SuitType suit;
        private int imageIndex;

        public int Rank { get => rank; set => rank = value; }
        public SuitType Suit { get => suit; }
        public int ImageIndex { get => imageIndex; }

        public Card()
        {

        }

        public Card(int rank, SuitType suit, int imageIndex)
        {
            this.rank = rank;
            this.suit = suit;
            this.imageIndex = imageIndex;
        }

        /// <summary>
        /// Gets the string representation of a card's ranksuit in the format of {rank}{first letter of suit}.
        /// </summary>
        /// <returns>A string representing the card's ranksuit.</returns>
        public string getRankSuit()
        {
            string rankSuit = string.Empty;
            string suitLetter = suit.ToString().Substring(0, 1);
            rankSuit = $"{rank}{suitLetter}";
            return rankSuit;
        }

        public override string ToString()
        {
            string cardName = "";
            string cardRank = "";

            switch (rank)
            {
                case 13:
                    cardRank = "King";
                    break;
                case 12:
                    cardRank = "Queen";
                    break;
                case 11:
                    cardRank = "Jack";
                    break;
                case 1:
                    cardRank = "Ace";
                    break;
                default:
                    cardRank = rank.ToString();
                    break;
            }
            cardName = cardRank + " of " + suit.ToString();
            return cardName;
        }

    }
}
