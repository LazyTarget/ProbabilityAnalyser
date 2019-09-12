using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			// todo: Only 2, 9 and ace

			var cards = new List<PlayingCard>();
			cards.Add(new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Nine));
			cards.Add(new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Ace));
			cards.Add(new PlayingCard(PlayingCardSuit.Diamonds, PlayingCardRank.Two));
			cards.Add(new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Two));

			cards.Add(new PlayingCard(PlayingCardSuit.Spades, PlayingCardRank.Nine));
			cards.Add(new PlayingCard(PlayingCardSuit.Spades, PlayingCardRank.Ace));
			cards.Add(new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Two));
			cards.Add(new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Nine));

			cards.Add(new PlayingCard(PlayingCardSuit.Spades, PlayingCardRank.Two));
			cards.Add(new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Ace));
			cards.Add(new PlayingCard(PlayingCardSuit.Diamonds, PlayingCardRank.Ace));
			cards.Add(new PlayingCard(PlayingCardSuit.Diamonds, PlayingCardRank.Nine));

			cards.Reverse();

			var deck = new PlayingCardDeck(cards);
			return deck;
		}


		[Test]
		public void TestDeck1()
		{
			var runner = new AcesUpRunner();
			runner.LoopTimes = 1;

			var wins = runner.RunMany((c, i) =>
			{
				c.GetDeck = GetDeck1;
			});

			Console.WriteLine($"{wins} wins out of {runner.LoopTimes} == {(wins / runner.LoopTimes):P4}");
			AssertMinWinPercentage(wins);
		}


		private void AssertMinWinPercentage(int wins, double percentage = AcesUpTests.MIN_WIN_PERCENTAGE)
		{
			var actual = wins / (double)percentage;
			Assert.IsTrue(actual > percentage, $"Did not reach target limit of {percentage:P2}, was: {actual:P2}");
		}
	}
}
