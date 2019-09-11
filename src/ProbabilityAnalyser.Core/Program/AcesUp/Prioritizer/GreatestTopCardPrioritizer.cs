using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer
{
	[DisplayName("by piles with highest top card")]
	public class GreatestTopCardPrioritizer : IPilePrioritizer
	{
		public virtual IEnumerable<AcesUpPile> Prioritize(AcesUpRunContext context)
		{
			var cards = context.FaceUpCards;
			var topCards = new Dictionary<int, PlayingCard>();
			if (cards.Pile1.LastOrDefault() != null)
				topCards.Add(1, cards.Pile1.Last());
			if (cards.Pile2.LastOrDefault() != null)
				topCards.Add(2, cards.Pile2.Last());
			if (cards.Pile3.LastOrDefault() != null)
				topCards.Add(3, cards.Pile3.Last());
			if (cards.Pile4.LastOrDefault() != null)
				topCards.Add(4, cards.Pile4.Last());

			var comparer = new PlayingCardRankComparer { AceRankHigh = true };
			var pairs = topCards.OrderByDescending(x => x.Value.Rank, comparer).ToList();
			foreach (var pair in pairs)
			{
				var pile = GetPileById(cards, pair.Key);
				if (pile != null)
					yield return pile;
			}
		}

		protected AcesUpPile GetPileById(AcesUpFaceUpCards cards, int id)
		{
			if (id == 1)
				return cards.Pile1;
			if (id == 2)
				return cards.Pile2;
			if (id == 3)
				return cards.Pile3;
			if (id == 4)
				return cards.Pile4;
			return null;
		}
	}
}
