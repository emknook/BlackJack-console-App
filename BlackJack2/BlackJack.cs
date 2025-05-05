using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack {
    public class BlackJack {
        private Deck d;
        private List<Player> players;
        private Hand dealer;
        private Boolean playing;
        public BlackJack() {
            Initialize();
            playing = true;
            while(playing) { 
                Console.Clear();
                PlayerRounds();
                Console.Clear();
                DealerFinish();
                Finish();
                Replay();
                if (players.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Thank you for playing!");
                    Console.WriteLine("Would you like to play again with different players?"); 
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "yes":
                        case "y":
                            Console.WriteLine("How fun!");
                            Initialize();
                            break;
                        default:
                            playing = false;
                            break;
                    }
                }
            }
        }

        private void Replay()
        {
            List<Player> playersToDelete = new List<Player>();
            foreach (Player p in players)
            {
                Console.WriteLine(p.GetName() + ", would you like to play another round?"); 
                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "yes":
                    case "y":
                        Console.WriteLine("How fun!");
                        p.GetHands().Clear();
                        break;
                    default:
                        FinishPlayer(p);
                        playersToDelete.Add(p);
                        break;
                }
            }
            foreach (Player p in playersToDelete)
            {
                players.Remove(p);
            }

        }

        private void FinishPlayer(Player p)
        {
            Console.WriteLine(p.GetName() + ", thank you for playing! You walk away with a balance of:" + p.GetBalance());

        }

        private void Finish() {
            int dealerPoints = dealer.GetPoints();
            foreach (Player p in players) {
                foreach (Hand h in p.GetHands()) {
                    Console.WriteLine(p.GetName() + "'s hand: " + h.ToString());
                    int points = h.GetPoints();
                    if (points <= 21) {
                        if (dealerPoints == 21 || dealerPoints == points) {

                            Console.WriteLine("Dealer has won... Your new balance is " + p.GetBalance());
                        }
                        else {
                            int winnings = h.GetBet() * 2;
                            p.AddBalance(winnings);
                            Console.WriteLine(winnings + " added to balance, new balance is: " + p.GetBalance());
                        }
                    }
                    else {
                        Console.WriteLine("Dead hand, your new balance is " + p.GetBalance());
                    }
                }
            }


        }

        private void DealerFinish() {
            Console.WriteLine("Dealer's " + dealer.ToString());
            while (dealer.Playing()) {
                if (dealer.GetPoints() >= 17) {
                    dealer.Stand();
                    break;
                }
                dealer.Add(d.GetCard(false));
                Console.WriteLine("Dealer's " + dealer.ToString());
            }
            Console.WriteLine("Dealer finished, let's see who won? (press enter)");
            Console.ReadLine();
        }

        private void PlayerRounds() {
            AskBets();
            dealer = new Hand(0);
            Console.WriteLine("Dealer gets two cards:");
            dealer.Add(d.GetCard(true));
            dealer.Add(d.GetCard(false));
            foreach (Player p in players)
            {
                foreach (Hand h in p.GetHands())
                {
                    Console.WriteLine(p.GetName());
                    h.Add(d.GetCard(false));
                    h.Add(d.GetCard(false));
                }
            }
            foreach (Player p in players) {
                Console.WriteLine(p.GetName() + ", it's your turn!");
                Round(p);
                Console.WriteLine("Next player's turn! press enter.");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private void Round(Player p) {
            List<Hand> addHands = new List<Hand>();
            List<Hand> removeHands = new List<Hand>();
            foreach (Hand h in p.GetHands()) {
                int currentBet = h.GetBet();
                Console.WriteLine(h.ToString());
                if (p.GetBalance() >= currentBet && h.GetPoints() != 21) {
                    if (AskDouble()) {
                        p.AddBalance(-currentBet);
                        h.SetBet(currentBet * 2);
                        h.Add(d.GetCard(false));
                    }
                    else {
                        Console.WriteLine("Not doubling bet");
                    }
                }
                while (h.Playing()) {
                    Tuple<List<Hand>, List<Hand>> t2 = AskMove(p, h);
                    addHands.AddRange(t2.Item1);
                    removeHands.AddRange(t2.Item2);

                }
            }
            foreach (Hand h in addHands) {
                p.AddHand(h);
            }
            foreach (Hand h in removeHands) {
                p.RemoveHand(h);
            }
        }

        private bool AskDouble() {
            Console.WriteLine("Would you like to double your hand? (adds a card)");
            string answer = Console.ReadLine();
            switch (answer) {
                case "yes":
                case "y":
                    return true;
                default:
                    return false;
            }
        }
        /***
         * 
         * Method returns 2 lists of hands, because you can't edit the list you're currently looping through. The lists will be used afterwards to add and remove the necessary hands, if a split has occurred.
         * Other option might've been to just toggle a switch in the card that says it's to be deleted? then remove with a sort of update method where it gets removed if it has that set to true;
         * 
         */


        private Tuple<List<Hand>, List<Hand>> AskMove(Player p, Hand h) {
            List<Hand> addHands = new List<Hand>();
            List<Hand> removeHands = new List<Hand>();
            Tuple<List<Hand>, List<Hand>> t = new Tuple<List<Hand>, List<Hand>>(addHands, removeHands);
            int points = h.GetPoints();
            Console.WriteLine(h.ToString());
            Console.WriteLine("What's your move? This hand's current points: " + points);
            Console.WriteLine("Pick one:");
            bool splittable = h.Splittable();
            if (splittable) {
                Console.WriteLine("Hit (h), split (sp), stand (st)");
            }
            else {
                Console.WriteLine("Hit (h), or stand (st)");
            }
            string answer = Console.ReadLine();
            switch (answer) {
                case "stand":
                case "st":
                    h.Stand();
                    break;
                case "split":
                case "sp":
                    if (!splittable) {
                        Console.WriteLine("Not splittable");
                        t = AskMove(p, h);
                        break;
                    }
                    else {
                        Card c1 = h.GetCards().First();
                        h.RemoveCard(c1);
                        Hand h1 = new Hand(h.GetBet());
                        h1.Add(c1);
                        h1.Add(d.GetCard(false));
                        addHands.Add(h1);
                        while (h1.Playing()) {
                            Tuple<List<Hand>, List<Hand>> t1 = AskMove(p, h1);
                            t.Item1.AddRange(t1.Item1);
                            removeHands.AddRange(t1.Item2);
                        }
                        Card c2 = h.GetCards().First();
                        h.RemoveCard(c2);
                        Hand h2 = new Hand(h.GetBet());
                        h2.Add(c2);
                        h2.Add(d.GetCard(false));
                        addHands.Add(h2);
                        while (h2.Playing()) {
                            Tuple<List<Hand>, List<Hand>> t2 = AskMove(p, h2);
                            t.Item1.AddRange(t2.Item1);
                            removeHands.AddRange(t2.Item2);
                        }
                        removeHands.Add(h); ;
                        h.Stand();
                    }
                    break;
                case "hit":
                case "h":
                    h.Add(d.GetCard(false));
                    break;
                default:
                    Console.WriteLine("Not an option");
                    return AskMove(p, h);
            }
            return t;
        }

        private void Initialize() {
            Console.WriteLine("How many decks in play? (1-4)");
            int amount = AskNumber(1, 4);
            d = new Deck(amount);
            players = new List<Player>();
            AddPlayers();
        }

        private void AskBets() {
            foreach (Player p in players) {
                Console.WriteLine(p.GetName() + ", how much do you want to bet on your first hand? Your balance is: " + p.GetBalance());
                int bet = AskNumber(1, p.GetBalance());
                p.GetHands().Add(new Hand(bet));
                p.AddBalance(-bet);
                Console.WriteLine("Bet set: " + bet + ", new balance is " + p.GetBalance());
            }
        }
        private void AddPlayers() {
            Console.WriteLine("How many players? (1-5)");
            int amount = AskNumber(1, 5);
            for (int i = 1; i <= amount; i++) {
                Console.WriteLine("What is player " + i + "'s name");
                players.Add(new Player(Console.ReadLine()));
            }
        }

        private int AskNumber(int start, int limit) {
            string input = Console.ReadLine();
            int number;
            if (!Int32.TryParse(input, out number)) {
                Console.WriteLine("Not a number... try again");
                return AskNumber(start, limit);
            }
            if (number < start || number > limit) {
                Console.WriteLine("Too low or too high... limit: " + limit);
                return AskNumber(start, limit);
            }
            return number;
        }
    }
}