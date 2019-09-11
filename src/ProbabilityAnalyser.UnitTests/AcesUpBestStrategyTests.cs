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
				Console.WriteLine($"{wins} wins\t :: {(wins/(double)NR_OF_INSTANCES):P2}\t\t Strategy: {args.FriendlyName}");
			}
		}
	}
}
