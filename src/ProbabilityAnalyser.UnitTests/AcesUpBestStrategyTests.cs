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

		private IPilePrioritizer[] _prioritizers = new IPilePrioritizer[]
		{
			new DefaultPilePrioritizer(),
			new GreatestTopCardPrioritizer(),
			new LargestPilePrioritizer(),
		};

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
			var builder = new AcesUpStrategyBuilder();


			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				)
			);
			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				)
			);
			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
					.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
				)
			);
			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<AcesToEmptyPiles>()
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
					.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
				)
			);
			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<AcesToEmptyPiles>()
					.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				)
			);
			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
					.AppendStrategy<AcesToEmptyPiles>()
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
				)
			);
			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
					.AppendStrategy<AcesToEmptyPiles>()
				)
			);
			combinations.AddRange(BuildForAllPrioritizers(
				builder
					.AppendStrategy<MoveFirstAvailableCardToEmptySpace>()
					.AppendStrategy<MoveCardBasedOnDirectlyUnderTopCard>()
					.AppendStrategy<AcesToEmptyPiles>()
				)
			);

			var distinct = combinations.GroupBy(x => x.FriendlyName).Select(x => x.First()).ToList();
			return distinct;
		}

		#endregion


		[Test]
		public void DetermineBestStrategy()
		{
			var combinations = new Dictionary<AcesUpArgCombination, int>();

			#region Combinations

			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move first available",
				MovingStrategy = new MoveFirstAvailableCardToEmptySpace()
			}, -1);
			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move first available :: largest pile prioritizer",
				MovingStrategy = new MoveFirstAvailableCardToEmptySpace(),
				Prioritizer = new LargestPilePrioritizer()
			}, -1);
			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move first available :: greatest top card prioritizer",
				MovingStrategy = new MoveFirstAvailableCardToEmptySpace(),
				Prioritizer = new GreatestTopCardPrioritizer()
			}, -1);

			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move based on card under otherwise move first available",
				MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(
					new MoveFirstAvailableCardToEmptySpace()
				)
			}, -1);
			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move based on card under otherwise move first available :: largest pile prioritizer",
				MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(
					new MoveFirstAvailableCardToEmptySpace()
				),
				Prioritizer = new LargestPilePrioritizer()
			}, -1);
			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move based on card under otherwise move first available :: greatest top card prioritizer",
				MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(
					new MoveFirstAvailableCardToEmptySpace()
				),
				Prioritizer = new GreatestTopCardPrioritizer()
			}, -1);

			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move first available or based on card under",
				MovingStrategy = new MoveFirstAvailableCardToEmptySpace(
					new MoveCardBasedOnDirectlyUnderTopCard()
				)
			}, -1);
			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move first available or based on card under :: largest pile prioritizer",
				MovingStrategy = new MoveFirstAvailableCardToEmptySpace(
					new MoveCardBasedOnDirectlyUnderTopCard()
				),
				Prioritizer = new LargestPilePrioritizer()
			}, -1);
			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move first available or based on card under :: greatest top card prioritizer",
				MovingStrategy = new MoveFirstAvailableCardToEmptySpace(
					new MoveCardBasedOnDirectlyUnderTopCard()
				),
				Prioritizer = new GreatestTopCardPrioritizer()
			}, -1);


			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move aces to empty piles, then move first available or based on card under",
				MovingStrategy = new AcesToEmptyPiles(
					new MoveFirstAvailableCardToEmptySpace(
						new MoveCardBasedOnDirectlyUnderTopCard()
					)
				)
			}, -1);

			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move aces to empty piles, then move based on card under otherwise move first available",
				MovingStrategy = new AcesToEmptyPiles(
					new MoveFirstAvailableCardToEmptySpace(
						new MoveCardBasedOnDirectlyUnderTopCard()
					)
				)
			}, -1);

			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move aces to empty piles, then move based on card under top otherwise move first available :: largest pile prioritizer",
				MovingStrategy = new AcesToEmptyPiles(
					new MoveFirstAvailableCardToEmptySpace(
						new MoveCardBasedOnDirectlyUnderTopCard()
					)
				),
				Prioritizer = new LargestPilePrioritizer()
			}, -1);

			combinations.Add(new AcesUpArgCombination
			{
				FriendlyName = $"Move aces to empty piles, then move based on card under top otherwise move first available :: greatest top card prioritizer",
				MovingStrategy = new AcesToEmptyPiles(
					new MoveCardBasedOnDirectlyUnderTopCard(
						new MoveFirstAvailableCardToEmptySpace()
					)
				),
				Prioritizer = new GreatestTopCardPrioritizer()
			}, -1);

			#endregion


			for (var i = 0; i < combinations.Count; i++)
			{
				var pair = combinations.ElementAtOrDefault(i);
				var args = pair.Key;
				var wins = RunTest(c => args.ApplyTo(c));
				combinations[args] = wins;
			}


			Console.WriteLine($"Out of {NR_OF_INSTANCES} instances, the following strategies where run:");

			var sorted = combinations.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
			for (var i = 0; i < sorted.Count; i++)
			{
				var pair = sorted.ElementAt(i);
				var args = pair.Key;
				var wins = pair.Value;
				Console.WriteLine($"{wins} wins\t :: {(wins / (double)NR_OF_INSTANCES):P2}\t\t Strategy: {args.FriendlyName}");
			}
		}


		[Test]
		public void DetermineBestStrategy2()
		{
			var combinations = FetchStrategyCombinations();

			var results = new Dictionary<AcesUpArgCombination, int>();
			for (var i = 0; i < combinations.Count; i++)
			{
				var args = combinations.ElementAt(i);
				var wins = RunTest(c => args.ApplyTo(c));
				results[args] = wins;
			}


			Console.WriteLine($"Out of {NR_OF_INSTANCES} instances, the following strategies where run:");

			var sorted = results.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
			for (var i = 0; i < sorted.Count; i++)
			{
				var pair = sorted.ElementAt(i);
				var args = pair.Key;
				var wins = pair.Value;
				Console.WriteLine($"{wins} wins\t :: {(wins / (double)NR_OF_INSTANCES):P2}\t\t Strategy: {args.FriendlyName}");
			}
		}
	}
}
