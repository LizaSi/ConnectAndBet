using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Deck
{
    private List<Card> cards;

    public int Count => cards.Count;

    public Deck(int numberOfDecks = 1)
    {
        cards = new List<Card>();
        for (int n = 0; n < numberOfDecks; n++)
        {
            foreach (CardsByOrder cardType in System.Enum.GetValues(typeof(CardsByOrder)))
            {
                cards.Add(new Card(cardType));
            }
        }
        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    public Card DrawCard()
    {
        if (cards.Count > 0)
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
        return null; // Consider reshuffling or reinitializing the deck here if empty
    }

    public List<Card> GetCards()
    {
        return new List<Card>(cards); // Return a copy of the list to prevent external modifications
    }
}


//public class Deck
//{
//    private List<Card> cards;

//    public int Count => cards.Count;  // This property returns the number of cards remaining in the deck.

//    public Deck(int numberOfDecks = 1)
//    {
//        cards = new List<Card>();
//        for (int n = 0; n < numberOfDecks; n++)
//        {
//            foreach (Card.Suit suit in System.Enum.GetValues(typeof(Card.Suit)))
//            {
//                foreach (Card.Rank rank in System.Enum.GetValues(typeof(Card.Rank)))
//                {
//                    cards.Add(new Card(suit, rank));
//                }
//            }
//        }
//        Shuffle();
//    }

//    public void Shuffle()
//    {
//        for (int i = cards.Count - 1; i > 0; i--)
//        {
//            int j = UnityEngine.Random.Range(0, i + 1); // Explicitly use UnityEngine.Random
//            Card temp = cards[i];
//            cards[i] = cards[j];
//            cards[j] = temp;
//        }
//    }

//    public Card DrawCard()
//    {
//        if (cards.Count > 0)
//        {
//            Card card = cards[0];
//            cards.RemoveAt(0);
//            return card;
//        }
//        return null; // Consider reshuffling or reinitializing the deck here if empty
//    }

//    public List<Card> GetCards()
//    {
//        return new List<Card>(cards); // Return a copy of the list to prevent external modifications
//    }
//}
