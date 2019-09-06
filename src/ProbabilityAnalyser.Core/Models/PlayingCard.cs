using System;
using System.Collections.Generic;
using System.Text;

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
	}
}
