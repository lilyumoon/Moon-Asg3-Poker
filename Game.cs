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
        public List<int> cardImageIndices = new List<int>();

        private Hand hand;
        private Deck deck;
        private PokerScore pokerScore;

        private int credits = 100;

        public int Credits { get => credits; set => credits = value; }

        public Game()
        {
            hand = new Hand();
            deck = new Deck();
            for (int i = 0; i < 5; i++)
            {
                heldStates.Add(false);
                cardImageIndices.Add(52);
            }
        }

        public void startRound(int bet)
        {
            // Deduct 'bet' number of credits
            deductCredits(bet);

            // Reset the deck and deal 5 cards from it
            deck.resetDeck();
            List<Card> dealtCards = deck.dealPokerHand();
            hand = new Hand(dealtCards);

            // Retain image indices of the drawn cards
            for (int i = 0; i < 5; i++)
            {
                heldStates[i] = false;
                cardImageIndices[i] = dealtCards[i].ImageIndex;
            }
        }

        public void completeRound(int bet, out string handResult, out int winnings)
        {
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

            List<string> rankSuits = hand.getRankSuits();
            pokerScore = new PokerScore(rankSuits[0], rankSuits[1], rankSuits[2], rankSuits[3], rankSuits[4]);

            handResult = pokerScore.scoreHand();

            winnings = bet * pokerScore.getPayoffRatio();
            if (winnings > 0)
                addCredits(winnings);
        }

        private void deductCredits(int amount)
        {
            credits -= amount;
        }

        private void addCredits(int amount)
        {
            credits += amount;
        }

        public void toggleCardHeldState(int cardIndex)
        {
            heldStates[cardIndex] = !heldStates[cardIndex];
        }


    }
}
