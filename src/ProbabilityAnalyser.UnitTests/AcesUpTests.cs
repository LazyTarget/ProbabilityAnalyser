using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program;

namespace ProbabilityAnalyser.UnitTests
{
	[TestClass]
	public class AcesUpTests
	{
		protected virtual int RunInstance()
		{
			var deck = PlayingCardDeck.Standard52CardDeck();

			var program = new AcesUp();
			var points = program.Run(deck);

			Console.WriteLine("AcesUp :: Result = {0}", points);
			return points;
		}


		[TestMethod]
		public void TestInstances()
		{
			int NR_OF_INSTANCES = 10;

			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				RunInstance();
			});
		}

	}
}
