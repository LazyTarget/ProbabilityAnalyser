using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
//using static ProbabilityAnalyser.UnitTests.AcesUpTests.AcesUpTests;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests
{
	[TestFixture]
	public class AcesUpSpecificGameTests
	{
		private PlayingCardDeck GetDeck1()
		{
			var cards = new List<PlayingCard>();


			return cards;
		}


		[Test]
		public void TestDeck1()
		{
			var runner = new AcesUpRunner();
			var wins = runner.RunMany((c, i) =>
			{
				c.GetDeck = GetDeck1;
			});

			Console.WriteLine($"{wins} wins out of {runner.LoopTimes} == {(wins / runner.LoopTimes):P4}");
			AssertMinWinPercentage(wins, (double)runner.LoopTimes);
		}


		private void AssertMinWinPercentage(int wins, double percentage)
		{
			var actual = wins / (double)percentage;
			Assert.IsTrue(actual > percentage, $"Did not reach target limit of {percentage:P2}, was: {actual:P2}");
		}
	}
}
