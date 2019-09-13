using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;
using ProbabilityAnalyser.UnitTests.AcesUpTests.Helpers;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests
{
	[TestFixture]
	public class AcesUpBestStrategyTests
	{
		#region Combinations

		private readonly IPilePrioritizer[] _prioritizers = new IPilePrioritizer[]
		{
			new DefaultPilePrioritizer(),
			new GreatestTopCardPrioritizer(),
			new LowestTopCardPrioritizer(),
			new LargestPilePrioritizer(),
			new SmallestPilePrioritizer(),
			new HasHiddenAcesPilePrioritizer(),
			new HasNoHiddenAcesPilePrioritizer(),
		};

		protected virtual void AddCombination(List<AcesUpTests.AcesUpArgCombination> list, Action<AcesUpStrategyBuilder> build)
		{
			var builder = new AcesUpStrategyBuilder();
			build(builder);
			var l = BuildForAllPrioritizers(builder);
			list.AddRange(l);
		}

		protected virtual IEnumerable<AcesUpTests.AcesUpArgCombination> BuildForAllPrioritizers(AcesUpStrategyBuilder builder)
		{
			foreach (var prioritizer in _prioritizers)
			{
				var combination = builder
					.Prioritizer(prioritizer)
					.Peek();
				yield return combination;
			}
		}

		protected virtual IList<AcesUpTests.AcesUpArgCombination> FetchStrategyCombinations()
		{
			var combinations = new List<AcesUpTests.AcesUpArgCombination>();


			AddCombination(combinations, builder => builder
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<AcesToEmptyPiles>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<AcesToEmptyPiles>()
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<AcesToEmptyPiles>()
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
				.AppendStrategy<AcesToEmptyPiles>()
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				.AppendStrategy<AcesToEmptyPiles>()
			);

			AddCombination(combinations, builder => builder
				.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
				.AppendStrategy<AcesToEmptyPiles>()
			);

			var distinct = combinations.GroupBy(x => x.FriendlyName).Select(x => x.First()).ToList();
			return distinct;
		}

		#endregion


		[Test]
		public void DetermineBestStrategy()
		{
			var instances = 1000; //AcesUpTests.NR_OF_INSTANCES;
			var combinations = FetchStrategyCombinations();

			var decks = new PlayingCardDeck[instances];
			CommonExtensions.InvokeLoop(true, instances, (i, s) =>
			{
				var deck = PlayingCardDeck.Standard52CardDeck();
				decks[i] = deck;
			});


			var results = new Dictionary<AcesUpTests.AcesUpArgCombination, int>();
			for (var i = 0; i < combinations.Count; i++)
			{
				var args = combinations.ElementAt(i);

				var runner = new AcesUpRunner();
				runner.LoopTimes = instances;
				runner.UseParallelLoops = AcesUpTests.PARALLEL_INSTANCES;

				var wins = runner.RunMany((c, idx) =>
				{
					c.GetDeck = () => new PlayingCardDeck(decks[idx].Cards);
				});
				//var wins = AcesUpTests.ExecuteTest(c => args.ApplyTo((AcesUpRunContext)c), instances, AcesUpTests.PARALLEL_INSTANCES);

				results[args] = wins;
			}


			Console.WriteLine($"Out of {instances} instances, the following {results.Count} strategy combinations where run:");

			var sorted = results.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
			for (var i = 0; i < sorted.Count; i++)
			{
				var pair = sorted.ElementAt(i);
				var args = pair.Key;
				var wins = pair.Value;
				Console.WriteLine($"{wins} wins\t :: {(wins / (double)instances):P2}\t\t Strategy: {args.FriendlyName}");
			}
		}
	}
}
