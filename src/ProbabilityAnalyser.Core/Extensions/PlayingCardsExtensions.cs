using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;

namespace ProbabilityAnalyser.Core.Extensions
{
	public static class PlayingCardsExtensions
	{
		public static string ToShortString(this PlayingCard card)
		{
			string str = "";
			if (card.Suit == PlayingCardSuit.Joker ||
			    card.Rank == PlayingCardRank.Joker)
			{
				str = "JJ";
			}
			else
			{
				string s = "";
				switch (card.Suit)
				{
					case PlayingCardSuit.Joker:
						s += "J";
						break;
					default:
						s += card.Suit.ToFriendlyString();
						break;
				}

				string r = "";
				switch (card.Rank)
				{
					case PlayingCardRank.Jack:
						r += "J";
						break;
					case PlayingCardRank.Queen:
						r += "Q";
						break;
					case PlayingCardRank.King:
						r += "K";
						break;
					case PlayingCardRank.Ace:
						r += "A";
						break;
					case PlayingCardRank.Joker:
						r += "J";
						break;

					default:
						var val = ((int) card.Rank).ToString();
						r += val;
						break;
				}

				while (r.Length < 2)
					r += " ";

				str = $"{s}{r}";
			}
			return str;
		}

		public static string ToFriendlyString(this PlayingCardSuit suit)
		{
			string str;
			if (suit == PlayingCardSuit.None)
			{
				str = suit.ToString();
			}
			else
			{
				switch (suit)
				{
					case PlayingCardSuit.Hearts:
						str = "♡";
						break;

					case PlayingCardSuit.Diamonds:
						str = "♦";
						break;

					case PlayingCardSuit.Spades:
						str = "♠";
						break;

					case PlayingCardSuit.Clubs:
						str = "♧";
						break;

					default:
						str = suit.ToString();
						break;
				}
			}
			return str;
		}

		public static string ToFriendlyString(this PlayingCardRank rank)
		{
			string str;
			if (rank == PlayingCardRank.None)
			{
				str = rank.ToString();
			}
			else if (rank == PlayingCardRank.Jack ||
			    rank == PlayingCardRank.Queen ||
			    rank == PlayingCardRank.King ||
			    rank == PlayingCardRank.Ace ||
			    rank == PlayingCardRank.Joker)
			{
				str = rank.ToString();
			}
			else
			{
				var val = (int)rank;
				str = val.ToString();
			}

			return str;
		}


		public static PlayingCard AllButHighestCardOfSuit(this IEnumerable<PlayingCard> cards, PlayingCardSuit suit, bool aceRankHigh = true)
		{
			var comparer = new PlayingCardRankComparer {AceRankHigh = aceRankHigh};
			var cardsOfSuit = cards.Where(x => x.Suit == suit).OrderByDescending(x => x.Rank, comparer).ToList();
			var result = cardsOfSuit.FirstOrDefault();
			return result;
		}


		public static void PopTopCard(this AcesUpPile pile)
		{
			PlayingCard card;
			PopTopCard(pile, out card);
		}

		public static void PopTopCard(this AcesUpPile pile, out PlayingCard card)
		{
			var cards = pile.Pile;
			var index = cards.Length - 1;
			card = cards[index];
			var result = cards.Take(index).ToArray();
			pile.Pile = result;
		}

		public static void AppendCard(this AcesUpPile pile, PlayingCard card)
		{
			var cards = pile.Pile;
			var result = cards.Concat(new[] { card }).ToArray();
			pile.Pile = result;
		}
	}
}
