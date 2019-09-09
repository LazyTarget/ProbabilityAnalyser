using System;
using System.IO;
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
		static AcesUpTests()
		{
			if (System.Diagnostics.Debugger.IsAttached)
				NR_OF_INSTANCES = 1;
		}

		public static int NR_OF_INSTANCES = 10000;
		public static bool PARALLEL_INSTANCES = false;



		protected virtual int RunTest(Action<AcesUp.AcesUpRunContext> configure = null)
		{
			var wins = RunTest(configure, NR_OF_INSTANCES, PARALLEL_INSTANCES);
			return wins;
		}

		protected virtual int RunTest(Action<AcesUp.AcesUpRunContext> configure, int instances, bool parallel)
		{
			double wins = 0;

			Action<int, ParallelLoopState> action = (i, s) =>
			{
				var pts = RunInstance(configure, i);
				if (pts > 48)
				{
					wins++;
				}
				else if (pts == 48)
				{
					// Final 4 cards are not Aces
				}
			};

			if (parallel)
			{
				var result = Parallel.For(0, instances, action);
			}
			else
			{
				for (var i = 0; i < instances; i++)
				{
					action(i, null);
				}
			}

			Console.WriteLine($"{wins} wins out of {instances} == {(wins / instances):P4}");
			return (int)wins;
		}


		protected virtual int RunInstance(Action<AcesUp.AcesUpRunContext> configure, int instanceId)
		{
			var deck = PlayingCardDeck.Standard52CardDeck();
			deck.Shuffle();

			var context = new AcesUp.AcesUpRunContext(deck, CancellationToken.None);
			configure?.Invoke(context);

			TextWriter output = null;

			//output = Console.Out;

			var program = new AcesUp(output, s => $"[{instanceId:D5}] :: {s}");

			var points = program.Run(context);

			//Console.WriteLine("AcesUp :: Result = {0}", points);
			return points;
		}



		[TestMethod]
		public void Strategy_MoveFirstAvailableCardToEmptySpace()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesUp.MoveFirstAvailableCardToEmptySpace();
					c.HardMode = false;
				}
			);
		}


		[TestMethod]
		public void Strategy_MoveFirstAvailableCardToEmptySpace_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesUp.MoveFirstAvailableCardToEmptySpace();
					c.HardMode = true;
				}
			);
		}



		[TestMethod]
		public void Strategy_MoveCardBasedOnDirectlyUnderTopCard_to_MoveFirstAvailableCardToEmptySpace()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesUp.MoveCardBasedOnDirectlyUnderTopCard(
						new AcesUp.MoveFirstAvailableCardToEmptySpace()
					);
					c.HardMode = false;
				}
			);
		}



		[TestMethod]
		public void Strategy_MoveCardBasedOnDirectlyUnderTopCard_to_MoveFirstAvailableCardToEmptySpace_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesUp.MoveCardBasedOnDirectlyUnderTopCard(
						new AcesUp.MoveFirstAvailableCardToEmptySpace()
					);
					c.HardMode = true;
				}
			);
		}

	}
}
