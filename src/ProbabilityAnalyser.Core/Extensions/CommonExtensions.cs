using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityAnalyser.Core.Extensions
{
	public class CommonExtensions
	{
		public static ParallelLoopResult? InvokeLoop(bool parallel, int times, Action<int, ParallelLoopState> loop)
		{
			ParallelLoopResult? result = null;
			if (parallel)
			{
				result = Parallel.For(0, times, loop);
			}
			else
			{
				for (var i = 0; i < times; i++)
				{
					loop(i, null);
				}
			}
			return result;
		}
	}
}
