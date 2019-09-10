using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer
{
	public class LargestPilePrioritizer : IPilePrioritizer
	{
		public IEnumerable<AcesUpPile> Prioritize(AcesUpRunContext context)
		{
			var sizes = new Dictionary<int, int>();
			sizes.Add(1, context.FaceUpCards.Pile1.Length);
			sizes.Add(2, context.FaceUpCards.Pile2.Length);
			sizes.Add(3, context.FaceUpCards.Pile3.Length);
			sizes.Add(4, context.FaceUpCards.Pile4.Length);

			var pairs = sizes.OrderByDescending(x => x.Value).ToList();
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
