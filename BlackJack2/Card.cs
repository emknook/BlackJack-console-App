
namespace BlackJack {
    class Card {
        private string suit;
        private string value;
        private int pointValue;

        public Card(string suit, string value, int pointValue) {
            this.suit = suit;
            this.value = value;
            this.pointValue = pointValue;
        }
        override
        public string ToString() {
            return suit + " " + value;
        }

        public int GetPointValue() {
            return pointValue;
        }
    }
}
