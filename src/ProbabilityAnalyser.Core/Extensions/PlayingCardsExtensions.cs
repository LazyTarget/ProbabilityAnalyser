using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProbabilityAnalyser.Core.Models;

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
				switch (card.Suit)
				{
					case PlayingCardSuit.Joker:
						str += "J";
						break;
					default:
						str += card.Suit.ToFriendlyString();
						break;
				}

				switch (card.Rank)
				{
					case PlayingCardRank.Jack:
						str += "J";
						break;
					case PlayingCardRank.Queen:
						str += "Q";
						break;
					case PlayingCardRank.King:
						str += "K";
						break;
					case PlayingCardRank.Ace:
						str += "A";
						break;
					case PlayingCardRank.Joker:
						str += "J";
						break;
					default:
						var val = (int) card.Rank;
						str += val.ToString();
						break;
				}
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


		public static PlayingCard[] PopTopCard(this PlayingCard[] cards)
		{
			PlayingCard card;
			var result = PopTopCard(cards, out card);
			return result;
		}

		public static PlayingCard[] PopTopCard(this PlayingCard[] cards, out PlayingCard card)
		{
			var index = cards.Length - 1;
			card = cards[index];
			var result = cards.Take(index).ToArray();
			return result;
		}
	}
}
