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
			deck.Shuffle();

			var program = new AcesUp();
			var points = program.Run(deck);

			//Console.WriteLine("AcesUp :: Result = {0}", points);
			return points;
		}


		[TestMethod]
		public void TestInstances()
		{
			int NR_OF_INSTANCES = 10000;

			double wins = 0;
			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				var pts = RunInstance();
				if (pts == 48)
				{
					wins++;
				}
			});

			Console.WriteLine($"{wins} wins out of {NR_OF_INSTANCES} == {(wins/NR_OF_INSTANCES):P}");
		}

	}
}
