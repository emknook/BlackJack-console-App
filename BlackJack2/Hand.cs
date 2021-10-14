
using System;
using System.Collections.Generic;

namespace BlackJack {
    class Hand {
        private int bet;
        private bool playing;

        private List<Card> cards;
        public Hand(int bet) {
            cards = new List<Card>();
            playing = true;
            this.bet = bet;
        }

        public void Add(Card c) {
            cards.Add(c);
        }

        public List<Card> GetCards() {
            return cards;
        }

        public void RemoveCard(Card c) {
            cards.Remove(c);
        }

        public void Stand() {
            playing = false;
        }

        public void SetBet(int b) {
            bet = b;
        }

        public int GetBet() {
            return bet;
        }

        public int GetPoints() {
            int i = 0;
            int aces = 0;
            foreach (Card c in cards) {
                if (c.GetPointValue() != 11) {
                    i += c.GetPointValue();
                }
                else {
                    aces++;
                }
            }
            if (i >= 11) {
                i += aces;
            }
            else if (aces >= 1 && (i + aces - 1) < 11) {
                i += 11;
                i += aces - 1;
            }
            return i;
        }

        public bool Playing() {
            if (GetPoints() >= 21) {
                playing = false;
                if (GetPoints() == 21) {
                    Console.WriteLine("Black Jack!!");
                }
                else {
                    Console.WriteLine("Dead");
                }
            }
            else if (cards.Count == 5) {
                playing = false;
            }
            return playing;
        }

        public bool Splittable() {
            var localCards = cards.ToArray();
            if (localCards.Length == 2 && localCards[0].GetPointValue() == localCards[1].GetPointValue()) {
                return true;
            }
            return false;
        }

        override
        public string ToString() {
            string cardsString = "";
            foreach (Card c in cards) {
                cardsString += c.ToString() + ", ";
            }
            return "Cards: " + cardsString + "points: " + GetPoints() + ", bet: " + GetBet();
        }

    }
}
