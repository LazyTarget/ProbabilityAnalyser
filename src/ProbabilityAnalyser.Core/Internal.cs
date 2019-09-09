using System;
using System.Collections.Generic;
using System.Text;

namespace ProbabilityAnalyser.Core
{
	internal static class Internal
	{
		static Internal()
		{
			Random = new Random();
		}

		public static readonly Random Random;
	}
}
