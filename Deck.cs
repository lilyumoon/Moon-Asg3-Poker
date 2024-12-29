using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_Asg3_Poker
{
    internal class Deck
    {
        private List<Card> cardList = new List<Card>();

        public Deck() { }

        /// <summary>
        /// Resets the Deck object's card list to a standard 52 playing card deck.
        /// </summary>
        public void resetToFullDeck()
        {
            clearDeck();
            int imgIndex = 0;

            // Need to load in order of A-K Clubs, Diamonds, Hearts, Spades (AAAA22223333...)

            for (int rank = 1; rank <= 13; rank++)
            {
                addCardToDeck(new Card(rank, Card.SuitType.Clubs, imgIndex));
                imgIndex++;

                addCardToDeck(new Card(rank, Card.SuitType.Diamonds, imgIndex));
                imgIndex++;

                addCardToDeck(new Card(rank, Card.SuitType.Hearts, imgIndex));
                imgIndex++;

                addCardToDeck(new Card(rank, Card.SuitType.Spades, imgIndex));
                imgIndex++;
            }
        }

        /// <summary>
        /// Adds a given card to the Deck object's card list.
        /// </summary>
        /// <param name="card">The card to add to the Deck object's card list.</param>
        public void addCardToDeck(Card card)
        {
            cardList.Add(card);
        }

        /// <summary>
        /// 'Draws' a random card out of the Deck object, 
        /// removes drawn card from the Deck's card list,
        /// and returns the 'drawn' card.
        /// </summary>
        /// <returns>The card drawn from the deck.</returns>
        public Card drawCardFromDeck()
        {
            Card card = new Card();

            // creates a Random object seeded from a hashed guid
            Random random = new Random(Guid.NewGuid().GetHashCode());

            // uses the Random object to select a card index
            //   between 0 and the number of cards in the deck
            int cardIndex = random.Next(0, cardList.Count);

            // loads the chosen card into the card object to be returned
            //   and removes it from the deck
            card = cardList[cardIndex];
            cardList.RemoveAt(cardIndex);

            return card;
        }

        // TODO: should this be moved to Hand.cs class?

        /// <summary>
        /// Replaces the card at a specified position in cardList with a new card,
        /// given that there is already a card in that position.
        /// </summary>
        /// <param name="cardIndex">The index (within cardList) of the card to replace.</param>
        /// <param name="newCard">The new Card object to replace the old Card object with.</param>
        public void replaceCardFromDeck(int cardIndex, Card newCard)
        {
            if (null != cardList.ElementAtOrDefault(cardIndex))
                cardList[cardIndex] = newCard;
        }

        /// <summary>
        /// Removes all cards from the Deck object's card list.
        /// </summary>
        public void clearDeck()
        {
            cardList.Clear();
        }

    }
}
