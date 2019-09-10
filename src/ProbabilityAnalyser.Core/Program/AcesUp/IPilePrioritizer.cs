using System;
using System.Collections.Generic;
using System.Text;

namespace ProbabilityAnalyser.Core.Program.AcesUp
{
	public interface IPilePrioritizer
	{
		IEnumerable<AcesUpPile> Prioritize(AcesUpRunContext context);
	}
}
