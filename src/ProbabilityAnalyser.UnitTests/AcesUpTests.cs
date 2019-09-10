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

namespace ProbabilityAnalyser.UnitTests
{
	[TestFixture]
	public class AcesUpTests
	{
		static AcesUpTests()
		{
			if (System.Diagnostics.Debugger.IsAttached)
				NR_OF_INSTANCES = 1;
		}

		public static int NR_OF_INSTANCES = 10000;
		public static bool PARALLEL_INSTANCES = false;



		internal static int RunTest(Action<AcesUpRunContext> configure = null)
		{
			var wins = RunTest(configure, NR_OF_INSTANCES, PARALLEL_INSTANCES);
			return wins;
		}

		internal static int RunTest(Action<AcesUpRunContext> configure, int instances, bool parallel)
		{
			TextWriter output = null;
			if (instances <= 1)
			{
				output = Console.Out;
			}

			double wins = 0;
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

			Console.WriteLine($"{wins} wins out of {instances} == {(wins / instances):P4}");
			return (int)wins;
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
		}

	}
}
