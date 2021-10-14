using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack {

    class Deck {

        private List<Card> cards;

        public Deck(int amount) {
            FillDeck(amount);
            ShuffleDeck();
        }

        private void FillDeck(int amount) {
            cards = new List<Card>();
            List<string> suits = new List<string> { "Hearts", "Spades", "Diamonds", "Clubs" };
            int cardsAmount = 0;
            for (int j = 0; j < amount; j++) {
                for (int i = 2; i <= 10; i++) {
                    foreach (string s in suits) {
                        cards.Add(new Card(s, "" + i, i));
                        cardsAmount++;
                    }
                }
                foreach (string s in suits) {
                    cards.Add(new Card(s, "Jack", 10));
                    cards.Add(new Card(s, "Queen", 10));
                    cards.Add(new Card(s, "King", 10));
                    cards.Add(new Card(s, "Ace", 11));
                    cardsAmount += 4;
                }
            }
            Console.WriteLine(cardsAmount + " cards, " + amount + " decks.");
        }

        public void ShuffleDeck() {
            var count = cards.Count;
            var last = count - 1;
            Random rand = new Random();
            for (var i = 0; i < last; i++) {
                var r = rand.Next(i, count);
                var tmp = cards[i];
                cards[i] = cards[r];
                cards[r] = tmp;
            }
        }

        public Card GetCard(bool firstDealerCard) {
            Card c = cards.First();
            cards.Remove(c);
            ShuffleDeck();
            if (firstDealerCard) {
                Console.WriteLine("Card given: upside down");
            }
            else {
                Console.WriteLine("Card given: " + c.ToString());
            }
            return c;
        }

    }

}
