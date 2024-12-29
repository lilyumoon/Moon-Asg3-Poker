using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class PokerScore
    {

        // Andy Bangsberg, for use with Lesson 4. Video Poker

        /// <summary>
        /// Used to return the results of Scoring a Hand
        /// Call the Constructor with five rankSuit strings:  12C, 1S, 7D, 8H, 9H
        /// Use the scoreHand method to get the results
        /// </summary>

        // Used to calculate hands, a Mini Card is just a Rank 1-13, and a Suit  C,H,S,D
        class MiniCard
        {
            public int rank { get; set; }
            public String suit { get; set; }

            public MiniCard(int rank, String suit)
            {
                this.rank = rank;
                this.suit = suit.ToUpper();
            }

        }

        // Member Variables
        private int payoutRatio;
        private String handResult = "Unknown";
        private List<MiniCard> listOfCards = new List<MiniCard>();

        // Constructor
        /// <summary>
        /// Expects a five strings each is a card string Rank + Suite
        ///         Must be Rank 1-13, Suite Must be C,H,S,D
        //  Example:  1C, 13H, 12S, 2H,  
        /// </summary>
        /// <param name="rankSuit1"></param>
        /// <param name="rankSuit2"></param>
        /// <param name="rankSuit3"></param>
        /// <param name="rankSuit4"></param>
        /// <param name="rankSuit5"></param>
        public PokerScore(String rankSuit1, String rankSuit2, String rankSuit3, String rankSuit4, String rankSuit5)
        {
            // split each rankSuit pair into rank and Suit

            listOfCards.Clear();

            int rank = 0;
            String suit = "";

            // Spit each rankSuite pair and add it as MiniCard   
            splitRankPair(rankSuit1, out rank, out suit);
            listOfCards.Add(new MiniCard(rank, suit));

            splitRankPair(rankSuit2, out rank, out suit);
            listOfCards.Add(new MiniCard(rank, suit));

            splitRankPair(rankSuit3, out rank, out suit);
            listOfCards.Add(new MiniCard(rank, suit));

            splitRankPair(rankSuit4, out rank, out suit);
            listOfCards.Add(new MiniCard(rank, suit));

            splitRankPair(rankSuit5, out rank, out suit);
            listOfCards.Add(new MiniCard(rank, suit));

        }

        // Methods
        public int getPayoffRatio()
        {
            return payoutRatio;
        }

        private void splitRankPair(String rankSuit, out int rank, out String suit)
        {
            // Input:   13S 
            // Output   rank = 13, suit = "S"
            suit = rankSuit.Substring(rankSuit.Length - 1, 1);
            int.TryParse(rankSuit.Replace(suit,""), out rank);

        }

        private bool isFourOfAKind()
        {
            int cardMatch = 0;
            bool isFourOfAKind = false;

            for (int cardRank = 1; cardRank < 14; cardRank++)
            {
                cardMatch = 0;
                foreach (MiniCard card in listOfCards)
                {
                    if (card.rank == cardRank)
                        cardMatch++;
                }

                if (cardMatch == 4)
                {
                    isFourOfAKind = true;
                    break;
                }

            }

            return isFourOfAKind;

        }

        private int getThreeOfAKindRank()
        {
            int cardMatch = 0;

            int rankMatched = 0;
            for (int cardRank = 1; cardRank < 14; cardRank++)
            {
                cardMatch = 0;
                foreach (MiniCard card in listOfCards)
                {
                    if (card.rank == cardRank)
                        cardMatch++;
                }

                if (cardMatch == 3)
                {
                    rankMatched = cardRank;
                    break;
                }

            }

            return rankMatched;

        }

        private int straightRank()
        {
            // returns  1 for stright, 2 for Ace high Straight
            int straightRank = 0;
            int[] arrayRanks = new int[5];
            int i = 0;
            String ranksInOrder = "";

            foreach (MiniCard card in listOfCards)
            {
                arrayRanks[i] = card.rank;
                i++;
            }

            // Sort and chck
            Array.Sort(arrayRanks);
            foreach (int rank in arrayRanks)
            {
                ranksInOrder = ranksInOrder + rank.ToString() + ",";
            }

            // Now that that we have something like "56789"  Check it agains
            if ("1,2,3,4,5,6,7,8,9,10,11,12,13,".IndexOf(ranksInOrder) > -1)
                straightRank = 1;
            else if ("1,10,11,12,13,".IndexOf(ranksInOrder) == 0)
                straightRank = 2;  // Ace High Straight, needed for royal flush

            return straightRank;

        }
        private int getTwoOfAKindRank(int firstPairRank)
        {
            // The first time call this with the argument of 0
            // isTwoOfAKind(0);

            // If we find 1 pair, the second time call with the rank of the first pair, so we don't re-hit the first pair.
            // check for A
            int cardMatch = 0;
            int rankMatched = 0;

            for (int cardRank = 1; cardRank < 14; cardRank++)
            {
                cardMatch = 0;
                foreach (MiniCard card in listOfCards)
                {
                    if (card.rank == cardRank && cardRank != firstPairRank)
                        cardMatch++;
                }

                if (cardMatch == 2)
                {
                    rankMatched = cardRank;
                    break;
                }

            }

            return rankMatched;
        }

        private bool isFlush()
        {
            bool isFlush = false;

            if (listOfCards[0].suit == listOfCards[1].suit &&
                listOfCards[1].suit == listOfCards[2].suit &&
                listOfCards[2].suit == listOfCards[3].suit &&
                listOfCards[3].suit == listOfCards[4].suit)
            {
                isFlush = true;
            }

            return isFlush;
        }



        public string scoreHand()
        {
             payoutRatio = -1;
             handResult = "";

            if (isFourOfAKind())
            {
                handResult = "Four of a Kind (50 to 1)";
                payoutRatio = 50;
            }
            else if (getThreeOfAKindRank() > 0)
            {
                int tripRank = getThreeOfAKindRank();
                int pairRank = getTwoOfAKindRank(tripRank);
                if (tripRank > 0 && pairRank > 0)
                {
                    handResult = "Full House (15 to 1)";
                    payoutRatio = 15;
                }

                else
                {
                    handResult = "Three of a kind (3 to 1)";
                    payoutRatio = 3;
                }

            }
            else if (isFlush())
            {
                if (straightRank() == 1)
                {
                    // Straight Flush, check for royal
                    handResult = "Straight Flush (500 to 1)";
                    payoutRatio = 500;
                }
                else if (straightRank() == 2)
                {
                    handResult = "Royal Flush (1000 to 1)";
                    payoutRatio = 1000;
                }
                else
                { 
                    handResult = "Flush (6 to 1)";
                    payoutRatio = 6;
                }

            }
            else if (straightRank() > 0)
            {
                handResult = "Straight (5 to 1)";
                payoutRatio = 5;
            }

            else
            {
                // Need to check for two par
                int firstPair = getTwoOfAKindRank(0);
                int secondPair = 0;
                if (firstPair > 0)
                    secondPair = getTwoOfAKindRank(firstPair);

                if (firstPair > 0 && secondPair > 0)
                {
                    handResult = "Two Pair (2 to 1)";
                    payoutRatio = 2;
                }

                else if (firstPair > 10 || firstPair == 1)
                {
                    if (firstPair == 11)
                        handResult = "Pair of Jacks (Even Money)";
                    else if (firstPair == 12)
                        handResult = "Pair of Queens (Even Money)";
                    else if (firstPair == 13)
                        handResult = "Pair of Kings (Even Money)";
                    else if (firstPair == 1)
                        handResult = "Pair of Aces (Even Money)";

                    payoutRatio = 1;
                }

                else
                {
                    handResult = "You lost your bet";
                    payoutRatio = 0;
                }


            }
           return handResult;
        }

    }

