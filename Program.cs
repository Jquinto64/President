using System;
using System.Linq;
using System.Collections.Generic;

using static System.Math;
using static System.Console;

// FOR LATER: RUNS, DOUBLE AND TRIPLE CARDS, REVERSE OUTCOME, AI'S, MORE PLAYERS

namespace prez
{
	// Define enumerated types for suits
	public enum Suit {Hearts, Clubs, Spades, Diamonds};
	
	// Class for card
	public class Card
	{
		// Card properties 
		public  Suit Suit {get; set;}
		public int Rank {get; set;}
		
		public Card (int Rank, Suit Suit)
		{
			this.Suit = Suit;
			this.Rank = Rank;
		}
		
		public override string ToString()
		{		
			// Display Value: rank as a string to the console
			// Rank should be b/w 3 and 14, where 2 is 3, 3 is 4..
			// B/w 2 to 10, just display rank
			// B/w 11 to 14, display the following: 
			// 11 should correspond to Jack, 12 should be Queen, 13 should be King, and 14 should be ace
			
			if (Rank >= 2 && Rank <= 9) return $"{Rank + 1} of {Suit}"; 
			else if (Rank == 10) return $"Jack of {Suit}";
			else if (Rank == 11) return $"Queen of {Suit}";
			else if (Rank == 12) return $"King of {Suit}";
			else if (Rank == 13) return $"Ace of {Suit}";
			else if (Rank == 14) return $"2 of {Suit}";
			
			return $"{Rank} of {Suit}";
		}
		
		public static Queue<Card> CreateDeck()
		{
			Queue<Card> deckList = new Queue<Card>();
			
			for (int j = 2; j <= 14; j++)
			{
				foreach (Suit suit in Enum.GetValues(typeof(Suit)))
				{
					deckList.Enqueue(new Card(j, suit));
				}
			}
			
			ShuffleDeck(deckList);
			return deckList;
		}
		
		// Shuffle method
        public static Queue<Card> ShuffleDeck(Queue<Card> x)
        {
			Random rGen = new Random();
			List<Card> deck = x.ToList();
			
			for (int i = deck.Count - 1; i > 0; --i)
			{
				int rand = rGen.Next(i + 1);
				Card temp = deck[i];
				deck[i] = deck[rand];
				deck[rand]= temp;
			}
			Queue<Card> newDeck = new Queue<Card>();
			
			foreach(Card y in deck) 
			{
				newDeck.Enqueue(y);
			}
			return newDeck;
		}
		
		// Dealing method
		public static Tuple <List<Card>, List<Card>> Deal2Hand(Queue<Card> x)
		{			
			List<Card> playerOne = new List<Card>();
			List<Card> playerTwo = new List<Card>();
			
			Queue<Card> d = ShuffleDeck(x);
			List<Card> deck = d.ToList();
			
			for (int i = 0; i <= (deck.Count/2) - 1; i++)
			{
				playerOne.Add(deck[i]);
			}
			
			for (int i = (deck.Count/2); i <= deck.Count - 1; i++)
			{
				playerTwo.Add(deck[i]);
			}

			List<Card> sortedPlayerOne = playerOne.OrderBy(o=>o.Rank).ToList();		
			List<Card> sortedPlayerTwo = playerTwo.OrderBy(o=>o.Rank).ToList();
			
			return Tuple.Create(sortedPlayerOne, sortedPlayerTwo);
		}
		
		public static void DisplayHands(List<Card> deck1, List<Card> deck2)
		{	
			WriteLine();
			WriteLine("Player One Hand:");
			WriteLine();

			for (int i = 0; i < deck1.Count; i++)
			{
				WriteLine($"{i}: {deck1[i]}");
			}

			WriteLine();
			WriteLine("Player Two Hand:");
			WriteLine();

			for (int i = 0; i < deck2.Count; i++)
			{
				WriteLine($"{i}: {deck2[i]}");
			}
		}

		public static void DisplayOneHand(List<Card> deck)
		{	
			for (int i = 0; i < deck.Count; i++)
			{
				WriteLine($"{i}: {deck[i]}");
			}
		}
	}
	
	public class Player
	{
		public List<Card> Hand {get; set;}
		
		// Method to select which card in their hand they want to play
		public static Card PlayCard(List<Card> Hand, int index)
		{
			Card play = Hand[index];
			return play;
		}
		public Player(List<Card> Hand)
		{
			this.Hand = Hand;
		}
	}
	
	public class GameManager
	{
		private Player Player1;
		private Player Player2;
		
		public Queue<Card> Deck {get; set;}
		public Stack<Card> UsedPile = new Stack<Card>();
		
		public GameManager()
		{
			// Create a List of Card objects (FULL DECK)
			Deck = Card.CreateDeck();
			
			// Create hands for each player 		
			List<Card> deckOne = new List<Card>();
			List<Card> deckTwo = new List<Card>();
			
			// Return both player's decks
			var dealt2Hands = Card.Deal2Hand(Deck);
			deckOne = dealt2Hands.Item1;
			deckTwo = dealt2Hands.Item2;
			
			// Instantiate player objects
			Player1 = new Player(deckOne);
			Player2 = new Player(deckTwo);
		}	
		
		public void PlayGame()
		{
			WriteLine("Welcome to the game of president!");

			
			// Press enter to play:
			WriteLine("Press enter to play");
			ReadLine();
			
			// Player with a 3 of spades puts down their card: more efficient way to do this with predicates?
			for (int i = 0; i < Player1.Hand.Count; i++)
			{
				if (Player1.Hand[i].Rank == 2 && Player1.Hand[i].Suit == Suit.Spades)
				{
					UsedPile.Push(Player1.Hand[i]);
					Player1.Hand.Remove(Player1.Hand[i]);
					WriteLine("3 of spades placed by player 1");
					WriteLine();
					// playerOneFirst = true;
				}
			}

			for (int i = 0; i < Player2.Hand.Count; i++)
			{
				if (Player2.Hand[i].Rank == 2 && Player2.Hand[i].Suit == Suit.Spades)
				{
					UsedPile.Push(Player2.Hand[i]);
					Player2.Hand.Remove(Player2.Hand[i]);
					WriteLine("3 of spades placed by player 2");
					WriteLine();
					// playerTwoFirst = true;
				}
			}

			// Card.DisplayHands(Player1.Hand, Player2.Hand);
			
			while (!(IsEndOfGame()))
			{
				WriteLine("Player 1's hand:");
				Card.DisplayOneHand(Player1.Hand);
				WriteLine();

				DisplayUsedPile(UsedPile);
				WriteLine();
			
				// Prompt to enter card
				WriteLine("Player 1: what card do you want to play?");
				int index1 = int.Parse(ReadLine());

				// Cards each player wants to play
				Card one = Player.PlayCard(Player1.Hand, index1);
				ProceedCard(one, Player1);

				WriteLine("Player 2's hand:");
				Card.DisplayOneHand(Player2.Hand);

				WriteLine();
				DisplayUsedPile(UsedPile);
				WriteLine();
				
				if (Player1.Hand.Count > 0)
				{
					WriteLine("Player 2: what card do you want to play?");
					// enter a string
					// split string
					// convert to ints
					int index2 = int.Parse(ReadLine());

					Card two = Player.PlayCard(Player2.Hand, index2);
					ProceedCard(two, Player2);

					WriteLine();
					WriteLine("Player 1's Hand:");
					Card.DisplayOneHand(Player1.Hand);	
					WriteLine();

					DisplayUsedPile(UsedPile);
					WriteLine();	
				}
				else break;
			}

			WriteLine("Game over!");
			if (Player1.Hand.Count == 0) Write(" Player 1 wins!");
			else if (Player2.Hand.Count == 0) Write(" Player 2 wins!");
		}

		public bool IsMovePossible(Player player, Card latestCardPlayed)
		{
			return player.Hand.Any(z => z.Rank > latestCardPlayed.Rank);
		}

		public static void DisplayUsedPile(Stack<Card> UsedPile)
		{
			Write("Used Card Pile:");
			Write($" {UsedPile.Peek()}");
		}

		public bool IsEndOfGame()
		{
			while (Player1.Hand.Count > 0 && Player2.Hand.Count > 0) 
			{
				return false;
			}
			return true;
		}
		
		// Actually plays the card each player selects
		public void ProceedCard(Card x, Player player)
		{
			Card latestCardPlayed = UsedPile.Peek();
			int index = 0;

			if (IsMovePossible(player, latestCardPlayed))
			{
				while (x.Rank < latestCardPlayed.Rank)
				{
					WriteLine("Card cannot be played; try again:");

					// Force user to play different card
					index = int.Parse(ReadLine());
					x = Player.PlayCard(player.Hand, index);
				}
				
				// CASE FOR BURN
				if (x.Rank == latestCardPlayed.Rank || x.Rank == 14)
				{
					WriteLine();
					WriteLine("Card is burned!");
					WriteLine();

					// Clear usedpile stack:
					UsedPile.Clear();
					Write("Used Card Pile:");
					Write(" Empty");
					WriteLine();

					// Remove card from hand
					player.Hand.Remove(x);

					// Display player's hand again
					WriteLine("Your hand:");
					WriteLine();
					Card.DisplayOneHand(player.Hand);

					// Allow the player to place a new card
					WriteLine();
					WriteLine("Enter a card to place in the used pile:");
					index = int.Parse(ReadLine());
					x = Player.PlayCard(player.Hand, index);

					while (x.Rank == 14)
					{
						WriteLine();
						WriteLine("Card is burned!");
						WriteLine();

						// Clear usedpile stack:
						UsedPile.Clear();
						Write("Used Card Pile:");
						Write(" Empty");
						WriteLine();

						// Remove card from hand
						player.Hand.Remove(x);

						// Display player's hand again
						WriteLine("Your hand:");
						WriteLine();
						Card.DisplayOneHand(player.Hand);

						// Allow the player to place a new card
						WriteLine();
						WriteLine("Enter a card to place in the used pile:");
						index = int.Parse(ReadLine());
						x = Player.PlayCard(player.Hand, index);
						}
						WriteLine();
				}

				else
				{
					WriteLine("No move possible, skipping turn");
					WriteLine();
				}
			}
			// Case for Run:
			// if (there are at least 3 cards in the discard and the three cards have consecutive ranks)
			// clear queue
			// each player selects a card to burn
			// private bool IsRun(Stack<Card> UsedPile)
			//{
			//	// check if last three cards in discard pile have consecutive ranks
			//
			//}

			UsedPile.Push(x);	
			player.Hand.Remove(x);
		}
	}
	
    static class Program
    {
        static void Main()
        {						
			GameManager game = new GameManager();
			while (!game.IsEndOfGame()) game.PlayGame();
        }
    }
}
