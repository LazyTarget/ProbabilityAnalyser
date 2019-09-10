using System;
using System.Collections.Generic;
using System.Text;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer
{
	public class DefaultPilePrioritizer : IPilePrioritizer
	{
		public IEnumerable<AcesUpPile> Prioritize(AcesUpRunContext context)
		{
			yield return context.FaceUpCards.Pile1;
			yield return context.FaceUpCards.Pile2;
			yield return context.FaceUpCards.Pile3;
			yield return context.FaceUpCards.Pile4;
		}
	}
}
