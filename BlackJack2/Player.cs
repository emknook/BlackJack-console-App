

using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack {
    class Player {

        private List<Hand> hands;
        private string name;
        private int balance;

        public Player(string name) {
            hands = new List<Hand>();
            this.name = name;
            balance = 100;
        }

        public string GetName() {
            return name;
        }

        public List<Hand> GetHands() {
            return hands;
        }

        public int GetBalance() {
            return balance;
        }

        public void AddBalance(int b) {
            balance += b;
        }

        public void AddHand(Hand h) {
            hands.Add(h);
        }

        public void RemoveHand(Hand h) {
            hands.Remove(h);
        }
    }
}
