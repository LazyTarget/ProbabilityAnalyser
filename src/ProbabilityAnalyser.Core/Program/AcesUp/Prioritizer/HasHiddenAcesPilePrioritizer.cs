using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer
{
	[DisplayName("by pile with hidden ace(s) as left to right")]
	public class HasHiddenAcesPilePrioritizer : IPilePrioritizer
	{
		public IEnumerable<AcesUpPile> Prioritize(AcesUpRunContext context)
		{
			var sizes = new Dictionary<int, int>();
			sizes.Add(1, context.FaceUpCards.Pile1.Reverse().Skip(1).Count(x => x.Rank == PlayingCardRank.Ace));
			sizes.Add(2, context.FaceUpCards.Pile2.Reverse().Skip(1).Count(x => x.Rank == PlayingCardRank.Ace));
			sizes.Add(3, context.FaceUpCards.Pile3.Reverse().Skip(1).Count(x => x.Rank == PlayingCardRank.Ace));
			sizes.Add(4, context.FaceUpCards.Pile4.Reverse().Skip(1).Count(x => x.Rank == PlayingCardRank.Ace));

			var pairs = sizes.OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToList();
			foreach (var pair in pairs)
			{
				var pile = GetPileById(context.FaceUpCards, pair.Key);
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
