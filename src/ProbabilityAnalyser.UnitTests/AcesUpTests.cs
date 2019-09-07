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
		public static int NR_OF_INSTANCES = 10000;


		protected virtual int RunInstance(Func<AcesUp.AcesUpRunContext, bool> movingStrategy)
		{
			var deck = PlayingCardDeck.Standard52CardDeck();
			deck.Shuffle();

			var program = new AcesUp();
			program.MovingStrategy = movingStrategy;

			var points = program.Run(deck);

			//Console.WriteLine("AcesUp :: Result = {0}", points);
			return points;
		}


		[TestMethod]
		public void Strategy_MoveFirstAvailableCardToEmptySpace()
		{
			double wins = 0;
			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				var pts = RunInstance(AcesUp.MoveFirstAvailableCardToEmptySpace);
				if (pts > 48)
				{
					wins++;
				}
				Console.WriteLine($"{pts} points");
			});

			Console.WriteLine($"{wins} wins out of {NR_OF_INSTANCES} == {(wins/NR_OF_INSTANCES):P}");
		}


		[TestMethod]
		public void Strategy_MoveCardBasedOnCardsUnderTop()
		{
			double wins = 0;
			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				var pts = RunInstance(AcesUp.MoveCardBasedOnCardsUnderTop);
				if (pts > 48)
				{
					wins++;
				}
				Console.WriteLine($"{pts} points");
			});

			Console.WriteLine($"{wins} wins out of {NR_OF_INSTANCES} == {(wins / NR_OF_INSTANCES):P}");
		}

	}
}
