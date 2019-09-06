using System;
using System.Collections.Generic;
using System.Text;

namespace ProbabilityAnalyser.Core.Models
{
	public class PlayingCard
	{
		public PlayingCard(PlayingCardSuit suit, PlayingCardRank rank)
		{
			Suit = suit;
			Rank = rank;
		}

		public PlayingCardSuit Suit { get; }
		public PlayingCardRank Rank { get; }
	}
}
