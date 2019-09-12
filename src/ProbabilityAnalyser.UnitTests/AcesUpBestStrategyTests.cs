using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;
using static ProbabilityAnalyser.UnitTests.AcesUpTests;

namespace ProbabilityAnalyser.UnitTests
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

		protected virtual void AddCombination(List<AcesUpArgCombination> list, Action<AcesUpStrategyBuilder> build)
		{
			var builder = new AcesUpStrategyBuilder();
			build(builder);
			var l = BuildForAllPrioritizers(builder);
			list.AddRange(l);
		}

		protected virtual IEnumerable<AcesUpArgCombination> BuildForAllPrioritizers(AcesUpStrategyBuilder builder)
		{
			foreach (var prioritizer in _prioritizers)
			{
				var combination = builder
					.Prioritizer(prioritizer)
					.Peek();
				yield return combination;
			}
		}

		protected virtual IList<AcesUpArgCombination> FetchStrategyCombinations()
		{
			var combinations = new List<AcesUpArgCombination>();


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
			var instances = NR_OF_INSTANCES;
			var combinations = FetchStrategyCombinations();

			var results = new Dictionary<AcesUpArgCombination, int>();
			for (var i = 0; i < combinations.Count; i++)
			{
				var args = combinations.ElementAt(i);
				var wins = ExecuteTest(c => args.ApplyTo(c), instances, PARALLEL_INSTANCES);
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
