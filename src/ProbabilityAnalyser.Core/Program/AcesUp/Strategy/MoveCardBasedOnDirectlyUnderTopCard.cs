using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Strategy
{
	[DisplayName("Based on card under top")]
	public class MoveCardBasedOnDirectlyUnderTopCard : CardMovingStrategyBase
	{
		public MoveCardBasedOnDirectlyUnderTopCard(ICardMovingStrategy fallback = null)
			: base(fallback)
		{
		}

		protected override bool Peek(AcesUpPile pile)
		{
			var context = pile.Context;
			var top = context.FaceUpCards.Top().ToArray();
			var suitsOnTop = top.Select(c => c.Suit).Distinct().ToArray();

			PlayingCard peek;
			PlayingCard card = null;
			if (pile.Length > 1)
			{
				if ((peek = pile.Reverse().ElementAtOrDefault(1)) != null && suitsOnTop.Contains(peek.Suit))
				{
					card = pile.LastOrDefault();
				}
			}

			return card != null;
		}
	}
}