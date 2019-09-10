using System;
using System.Linq;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Strategy
{
	public class MoveFirstAvailableCardToEmptySpace : CardMovingStrategyBase
	{
		public MoveFirstAvailableCardToEmptySpace(ICardMovingStrategy fallback = null)
			: base(fallback)
		{
		}

		protected override bool Peek(PlayingCard[] pile)
		{
			PlayingCard peek;
			PlayingCard card = null;
			if (pile.Length > 1 && (peek = pile.Last()) != null)
			{
				card = peek;
			}

			return card != null;
		}
	}
}