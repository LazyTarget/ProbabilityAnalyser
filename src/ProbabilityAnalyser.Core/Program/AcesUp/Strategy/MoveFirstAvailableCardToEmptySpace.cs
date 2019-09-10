using System;
using System.Collections.Generic;
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

		protected override IEnumerable<AcesUpPile> Prioritize(AcesUpRunContext context)
		{
			yield return context.FaceUpCards.Pile1;
			yield return context.FaceUpCards.Pile2;
			yield return context.FaceUpCards.Pile3;
			yield return context.FaceUpCards.Pile4;
		}

		protected override bool Peek(AcesUpPile pile)
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