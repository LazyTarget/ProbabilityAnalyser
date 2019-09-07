using System;
using System.Threading;
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


		protected virtual int RunInstance(Action<AcesUp.AcesUpRunContext> configure = null)
		{
			var deck = PlayingCardDeck.Standard52CardDeck();
			deck.Shuffle();

			var context = new AcesUp.AcesUpRunContext(deck, CancellationToken.None);
			configure?.Invoke(context);

			var program = new AcesUp();

			var points = program.Run(context);

			//Console.WriteLine("AcesUp :: Result = {0}", points);
			return points;
		}


		[TestMethod]
		public void Strategy_MoveFirstAvailableCardToEmptySpace()
		{
			double wins = 0;
			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				var pts = RunInstance(c =>
				{
					c.MovingStrategy = AcesUp.MoveFirstAvailableCardToEmptySpace;
				});
				if (pts > 48)
				{
					wins++;
				}
				Console.WriteLine($"{pts} points");
			});

			Console.WriteLine($"{wins} wins out of {NR_OF_INSTANCES} == {(wins/NR_OF_INSTANCES):P}");
		}


		[TestMethod]
		public void Strategy_MoveFirstAvailableCardToEmptySpace_Hard()
		{
			double wins = 0;
			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				var pts = RunInstance(c =>
				{
					c.MovingStrategy = AcesUp.MoveFirstAvailableCardToEmptySpace;
					c.HardMode = true;
				});
				if (pts > 48)
				{
					wins++;
				}
				Console.WriteLine($"{pts} points");
			});

			Console.WriteLine($"{wins} wins out of {NR_OF_INSTANCES} == {(wins / NR_OF_INSTANCES):P}");
		}


		[TestMethod]
		public void Strategy_MoveCardBasedOnCardsUnderTop()
		{
			double wins = 0;
			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				var pts = RunInstance(c =>
				{
					c.MovingStrategy = AcesUp.MoveCardBasedOnCardsUnderTop;
				});
				if (pts > 48)
				{
					wins++;
				}
				Console.WriteLine($"{pts} points");
			});

			Console.WriteLine($"{wins} wins out of {NR_OF_INSTANCES} == {(wins / NR_OF_INSTANCES):P}");
		}



		[TestMethod]
		public void Strategy_MoveCardBasedOnCardsUnderTop_HardMode()
		{
			double wins = 0;
			Parallel.For(0, NR_OF_INSTANCES, (i, s) =>
			{
				var pts = RunInstance(c =>
				{
					c.MovingStrategy = AcesUp.MoveCardBasedOnCardsUnderTop;
					c.HardMode = true;
				});
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
