using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_Asg3_Poker
{
    public class CardContainer
    {
        protected List<Card> cards = new List<Card>();

        /// <summary>
        /// Adds a given card to the cards list.
        /// </summary>
        /// <param name="card">The card to add to the card list.</param>
        protected virtual void addCard(Card card)
        {
            cards.Add(card);
        }

        /// <summary>
        /// Removes all cards from the cards list.
        /// </summary>
        protected virtual void clearCards()
        {
            cards.Clear();
        }
    }
}
