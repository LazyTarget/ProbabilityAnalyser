using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests
{
	[TestFixture]
	public class AcesUpTests : TestBase
	{
		static AcesUpTests()
		{
			if (System.Diagnostics.Debugger.IsAttached)
				NR_OF_INSTANCES = 1;
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
		}

		public static int NR_OF_INSTANCES = 10000;
		public static bool PARALLEL_INSTANCES = false;
		public const double MIN_WIN_PERCENTAGE = 0.005;



		public static int RunTest(Action<AcesUpRunContext> configure = null)
		{
			var wins = RunTest(configure, NR_OF_INSTANCES, PARALLEL_INSTANCES);
			return wins;
		}

		public static int RunTest(Action<AcesUpRunContext> configure, int instances, bool parallel)
		{
			var wins = (double)ExecuteTest(configure, instances, parallel);
			Console.WriteLine($"{wins} wins out of {instances} == {(wins / instances):P4}");
			return (int)wins;
		}

		internal static int ExecuteTest(Action<AcesUpRunContext> configure, int instances, bool parallel)
		{
			TextWriter output = null;
			if (instances <= 1)
			{
				output = Console.Out;
			}

			var wins = 0;
			Action<int, ParallelLoopState> action = (i, s) =>
			{
				Action<AcesUpRunContext> conf = (ctx) =>
				{
					configure(ctx);
				};

				var pts = RunInstance(conf, output, i);
				if (pts > 48)
				{
					wins++;
				}
				else if (pts == 48)
				{
					// Final 4 cards are not Aces
				}


				if (instances == 1)
				{
					output?.WriteLine($"Points: {pts}");
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
			return wins;
		}


		internal static int RunInstance(Action<AcesUpRunContext> configure, TextWriter output, int instanceId)
		{
			var deck = PlayingCardDeck.Standard52CardDeck();
			deck.Shuffle();

			var context = new AcesUpRunContext(deck, CancellationToken.None);
			configure?.Invoke(context);

			var program = new AcesUp(output, s =>
			{
				if (instanceId > 1)
					return $"[{instanceId:D5}] :: {s}";
				return s;
			});

			var points = program.Run(context);

			//Console.WriteLine("AcesUp :: Result = {0}", points);
			return points;
		}

		private void AssertMinWinPercentage(int wins, double percentage = MIN_WIN_PERCENTAGE)
		{
			var actual = wins / (double)percentage;
			Assert.IsTrue(actual > percentage, $"Did not reach target limit of {percentage:P2}, was: {actual:P2}");
		}



		[Test]
		public void Strategy_MoveFirstAvailableCardToEmptySpace()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new MoveFirstAvailableCardToEmptySpace();
					c.HardMode = false;
				}
			);
			AssertMinWinPercentage(wins);
		}


		[Test]
		public void Strategy_MoveFirstAvailableCardToEmptySpace_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new MoveFirstAvailableCardToEmptySpace();
					c.HardMode = true;
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_MoveCardBasedOnDirectlyUnderTopCard_to_MoveFirstAvailableCardToEmptySpace()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(
						new MoveFirstAvailableCardToEmptySpace()
					);
					c.HardMode = false;
				}
			);
			AssertMinWinPercentage(wins);
		}


		[Test]
		public void Strategy_MoveCardBasedOnDirectlyUnderTopCard_to_MoveFirstAvailableCardToEmptySpace_with_largest_pile_prioritizer()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(
						new MoveFirstAvailableCardToEmptySpace()
					);
					c.HardMode = false;
					c.Prioritizer = new LargestPilePrioritizer();
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_MoveCardBasedOnDirectlyUnderTopCard_to_MoveFirstAvailableCardToEmptySpace_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(
						new MoveFirstAvailableCardToEmptySpace()
					);
					c.HardMode = true;
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_AcesToEmptyPiles()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesToEmptyPiles();
					c.HardMode = false;
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_AcesToEmptyPiles_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesToEmptyPiles();
					c.HardMode = true;
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_AcesToEmptyPiles_to_MoveFirstAvailableCardToEmptySpace()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesToEmptyPiles(
						new MoveFirstAvailableCardToEmptySpace()
					);
					c.HardMode = false;
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_AcesToEmptyPiles_to_MoveFirstAvailableCardToEmptySpace_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesToEmptyPiles(
						new MoveFirstAvailableCardToEmptySpace()
					);
					c.HardMode = true;
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_AcesToEmptyPiles_to_MoveCardBasedOnDirectlyUnderTopCard_to_MoveFirstAvailableCardToEmptySpace()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesToEmptyPiles(
						new MoveCardBasedOnDirectlyUnderTopCard(
							new MoveFirstAvailableCardToEmptySpace()
						)
					);
					c.HardMode = false;
				}
			);
			AssertMinWinPercentage(wins);
		}



		[Test]
		public void Strategy_AcesToEmptyPiles_to_MoveCardBasedOnDirectlyUnderTopCard_to_MoveFirstAvailableCardToEmptySpace_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new AcesToEmptyPiles(
						new MoveCardBasedOnDirectlyUnderTopCard(
							new MoveFirstAvailableCardToEmptySpace()
						)
					);
					c.HardMode = true;
				}
			);
			AssertMinWinPercentage(wins);
		}


		[Test]
		public void Strategy_MoveCardBasedOnDirectlyUnderTopCard_to_AcesToEmptyPiles_to_MoveFirstAvailableCardToEmptySpace_when_hard_mode()
		{
			var wins = RunTest(
				c =>
				{
					c.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(
						new AcesToEmptyPiles(
							new MoveFirstAvailableCardToEmptySpace()
						)
					);
					c.HardMode = true;
				}
			);
			AssertMinWinPercentage(wins);
		}



		public class AcesUpArgCombination : AcesUpArguments
		{
			public string FriendlyName { get; set; }
		}
	}
}
