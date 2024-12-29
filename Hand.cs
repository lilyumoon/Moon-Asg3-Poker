using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_Asg3_Poker
{
    internal class Hand : CardContainer
    {

        public Hand() { }

        public Hand(List<Card> cards)
        {
            this.cards = cards;
        }

        /// <summary>
        /// Replaces the card at a specified position in cardList with a new card,
        /// given that there is already a card in that position.
        /// </summary>
        /// <param name="cardIndex">The index (within cardList) of the card to replace.</param>
        /// <param name="newCard">The new Card object to replace the old Card object with.</param>
        public void replaceCard(int cardIndex, Card newCard)
        {
            if (null != cards.ElementAtOrDefault(cardIndex))
                cards[cardIndex] = newCard;
        }

        public List<string> getRankSuits()
        {
            List<string> rankSuits = new List<string>
            {
                cards[0].getRankSuit(),
                cards[1].getRankSuit(),
                cards[2].getRankSuit(),
                cards[3].getRankSuit(),
                cards[4].getRankSuit()
            };
            return rankSuits;
        }

    }
}
