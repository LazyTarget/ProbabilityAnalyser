using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProbabilityAnalyser.Core.Program.AcesUp
{
	public class AcesUpArguments
	{
		public bool HardMode { get; set; }
		public ICardMovingStrategy MovingStrategy;
		public IPilePrioritizer Prioritizer;


		public void ApplyTo(AcesUpArguments arguments)
		{
			arguments.HardMode = HardMode;
			arguments.MovingStrategy = MovingStrategy;
			arguments.Prioritizer = Prioritizer;
		}

		public void ApplyTo(AcesUpRunContext context)
		{
			context.HardMode = HardMode;
			context.MovingStrategy = MovingStrategy;
			context.Prioritizer = Prioritizer;
		}
	}
}
