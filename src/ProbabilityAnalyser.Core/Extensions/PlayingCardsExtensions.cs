using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Extensions
{
	public static class PlayingCardsExtensions
	{
		public static string ToFriendlyString(this PlayingCardSuit suit)
		{
			string str;
			if (suit == PlayingCardSuit.None)
			{
				str = suit.ToString();
			}
			else
			{
				str = suit.ToString();
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

	}
}
