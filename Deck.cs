using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_Asg3_Poker
{
    internal class Deck : CardContainer
    {
        public Deck() { }

        /// <summary>
        /// Sets the cards list contents to a standard 52 playing card deck.
        /// </summary>
        public void resetDeck()
        {
            clearCards();
            int imgIndex = 0;

            // Need to load in order of A-K Clubs, Diamonds, Hearts, Spades (AAAA22223333...)
            for (int rank = 1; rank <= 13; rank++)
            {
                addCard(new Card(rank, Card.SuitType.Clubs, imgIndex));
                imgIndex++;

                addCard(new Card(rank, Card.SuitType.Diamonds, imgIndex));
                imgIndex++;

                addCard(new Card(rank, Card.SuitType.Hearts, imgIndex));
                imgIndex++;

                addCard(new Card(rank, Card.SuitType.Spades, imgIndex));
                imgIndex++;
            }
        }

        /// <summary>
        /// 'Draws' a random card out of the Deck object, 
        /// removes drawn card from the Deck's card list,
        /// and returns the 'drawn' card.
        /// </summary>
        /// <returns>The card drawn from the deck.</returns>
        public Card drawCard()
        {
            Card card = new Card();

            // creates a Random object seeded from a hashed guid
            Random random = new Random(Guid.NewGuid().GetHashCode());

            // uses the Random object to select a card index
            //   between 0 and the number of cards in the deck
            int cardIndex = random.Next(0, cards.Count);

            // loads the chosen card into the card object to be returned
            //   and removes it from the deck
            card = cards[cardIndex];
            cards.RemoveAt(cardIndex);

            return card;
        }

        /// <summary>
        /// Deals a 5 card poker hand.
        /// </summary>
        /// <returns>A list containing 5 card objects.</returns>
        public List<Card> dealPokerHand()
        {
            List<Card> pokerHand = new List<Card>();
            while (pokerHand.Count < 5)
            {
                Card card = drawCard();
                pokerHand.Add(card);
            }
            return pokerHand;
        }

    }
}
