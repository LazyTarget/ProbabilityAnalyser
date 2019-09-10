using System;
using System.Collections.Generic;
using System.Linq;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Strategy
{
	public class AcesToEmptyPiles : CardMovingStrategyBase
	{
		public AcesToEmptyPiles(ICardMovingStrategy fallback = null)
			: base(fallback)
		{
		}

		protected override bool Peek(AcesUpPile pile)
		{
			PlayingCard peek;
			PlayingCard card = null;
			if (pile.Length > 1 && (peek = pile.Last()) != null && peek.Rank == PlayingCardRank.Ace &&
			    (peek = pile.LastOrDefault()) != null)
			{
				card = peek;
			}
			return card != null;
		}
	}
}