using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Moon_Asg3_Poker
{
    internal class Game
    {
        /// <summary>
        /// The list of bools correlating to the 'held' state of each Card in the cards list.
        /// </summary>
        private List<bool> heldStates = new List<bool>();

        // Holds a record of the card image indices for each hand
        // so the Form can access them for UI updates
        private List<int> cardImageIndices = new List<int>();

        public List<int> CardImageIndices { get => cardImageIndices; }

        private Hand hand;
        private Deck deck;
        private PokerScore pokerScore;

        private int credits = 100;
        public int Credits { get => credits; }

        /// <summary>
        /// Stores all possible handResult messages from PokerScore's scoreHand method in a list.
        /// </summary>
        private List<string> possibleHandResults = new List<string>
        {
            "Royal Flush (1000 to 1)",
            "Straight Flush (500 to 1)",
            "Four of a Kind (50 to 1)",
            "Full House (15 to 1)",
            "Flush (6 to 1)",
            "Straight (5 to 1)",
            "Three of a kind (3 to 1)",
            "Two Pair (2 to 1)",
            "Pair of Jacks (Even Money)",
            "Pair of Queens (Even Money)",
            "Pair of Kings (Even Money)",
            "Pair of Aces (Even Money)",
            "You lost your bet"
        };

        public List<string> PossibleHandResults { get => possibleHandResults; }

        public Game()
        {
            // initialize hand and deck
            hand = new Hand();
            deck = new Deck();

            // populate the heldStates and cardImageIndices lists with 5 items each
            for (int i = 0; i < 5; i++)
            {
                heldStates.Add(false);
                cardImageIndices.Add(52);
            }
        }

        /// <summary>
        /// Method to handle the game-side logic when the 'bet' button is pressed.
        /// Deducts 'bet' number of credits and deals a 5 card poker hand.
        /// </summary>
        /// <param name="bet">The number of credits bet.</param>
        public void startRound(int bet)
        {
            // Deduct 'bet' number of credits
            deductCredits(bet);

            // Reset (shuffle) the deck and deal 5 cards from it
            deck.resetDeck();
            List<Card> dealtCards = deck.dealPokerHand();
            hand = new Hand(dealtCards);

            // Reset held states and retain image indices of the drawn cards
            for (int i = 0; i < 5; i++)
            {
                heldStates[i] = false;
                cardImageIndices[i] = dealtCards[i].ImageIndex;
            }
        }

        /// <summary>
        /// Method to handle the game-side logic when the 'draw' button is pressed.
        /// Signals to the Hand.cs class if any cards should be replaced.
        /// Uses the PokerScore class to score the resulting hand and passes the
        /// winnings and hand result message out. Adds winnings to credits.
        /// </summary>
        /// <param name="bet">The number of credits bet.</param>
        /// <param name="handResult">The result of the hand.</param>
        /// <param name="winnings">The number of credits won (if any).</param>
        public void completeRound(int bet, out string handResult, out int winnings)
        {
            // Replace any cards not marked to be held with new cards
            for (int i = 0; i < heldStates.Count; i++)
            {
                bool shouldBeReplaced = !heldStates[i];
                if (shouldBeReplaced)
                {
                    Card card = deck.drawCard();
                    cardImageIndices[i] = card.ImageIndex;
                    hand.replaceCard(i, card);
                }
            }

            // Convert cards to rankSuit representation for scoring by PokerScore
            List<string> rankSuits = hand.getRankSuits();
            pokerScore = new PokerScore(rankSuits[0], rankSuits[1], rankSuits[2], rankSuits[3], rankSuits[4]);

            // Get the hand result message from PokerScore
            //   (which will be passed out for retrieval by Form for UI updates)
            handResult = pokerScore.scoreHand();

            // Calculate winnings based on payout ratio and bet, and add credits if appropriate
            //   (which will be passed out for retrieval by Form for UI updates)
            winnings = bet * pokerScore.getPayoffRatio();
            if (winnings > 0)
                addCredits(winnings);
        }

        /// <summary>
        /// Remove a number of credits from the total.
        /// </summary>
        /// <param name="number">The number of credits to remove.</param>
        private void deductCredits(int number)
        {
            credits -= number;
        }

        /// <summary>
        /// Add a number of credits to the total.
        /// </summary>
        /// <param name="number">The number of credits to add.</param>
        private void addCredits(int number)
        {
            credits += number;
        }

        /// <summary>
        /// Inverts a card's held state at a specified position.
        /// </summary>
        /// <param name="cardIndex">The card position/index that should have its held state toggled.</param>
        public void toggleCardHeldState(int cardIndex)
        {
            heldStates[cardIndex] ^= true;
        }


    }
}
