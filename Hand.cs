using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_Asg3_Poker
{
    internal class Hand : CardContainer
    {
        /// <summary>
        /// The list of bools correlating to the 'held' state of each Card in the cards list.
        /// </summary>
        private List<bool> heldStates = new List<bool>();

        public Hand() { }

        public Hand(List<Card> cards)
        {
            this.cards = cards;
            foreach (Card card in cards)
                heldStates.Add(false);
        }

        public void toggleCardHeldState(int cardListIndex)
        {
            heldStates[cardListIndex] = !heldStates[cardListIndex];
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


    }
}
