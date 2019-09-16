using System;
using System.Collections.Generic;
using System.Text;
using ProbabilityAnalyser.Core.Extensions;

namespace ProbabilityAnalyser.Core.Models
{
	public class PlayingCard
	{
		public static readonly PlayingCard Joker = new PlayingCard(PlayingCardSuit.Joker, PlayingCardRank.Joker);


		public PlayingCard(PlayingCardSuit suit, PlayingCardRank rank)
		{
			Suit = suit;
			Rank = rank;
		}

		public PlayingCardSuit Suit { get; }
		public PlayingCardRank Rank { get; }


		// todo: Implement IEquatable<PlayingCard> to check Suit and Rank, if is equivalent card


		public override string ToString()
		{
			string str;
			if (Suit == PlayingCardSuit.Joker ||
			    Rank == PlayingCardRank.Joker)
			{
				str = "Joker";
			}
			else if (Suit != PlayingCardSuit.None &&
			         Rank != PlayingCardRank.None)
			{
				//var s = Suit.ToFriendlyString();
				//var r = Rank.ToFriendlyString();

				////str = $"{Rank} {Suit}";
				//str = $"{r} of {s}";

				str = this.ToShortString();
			}
			else
			{
				str = base.ToString();
			}
			return str;
		}
	}
}
