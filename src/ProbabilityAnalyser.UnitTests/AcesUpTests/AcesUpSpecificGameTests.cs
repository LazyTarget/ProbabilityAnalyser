using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
//using static ProbabilityAnalyser.UnitTests.AcesUpTests.AcesUpTests;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests
{
	[TestFixture]
	public class AcesUpSpecificGameTests : TestBase
	{
		public static PlayingCardDeck GetDeck1()
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

		public static PlayingCardDeck GetDeck2()
		{
			var settings = new JsonSerializerSettings();
			settings.Formatting = Formatting.None;
			settings.Converters.Add(new StringEnumConverter());


			string json;
			PlayingCardDeck deck;

			// Generate new..
			//deck = PlayingCardDeck.Standard52CardDeck();
			//deck.Shuffle();
			//json = JsonConvert.SerializeObject(deck.Cards, settings);

			//json = "[{\"Suit\":\"Clubs\",\"Rank\":\"Two\"},{\"Suit\":\"Clubs\",\"Rank\":\"Three\"},{\"Suit\":\"Clubs\",\"Rank\":\"Four\"},{\"Suit\":\"Clubs\",\"Rank\":\"Five\"},{\"Suit\":\"Clubs\",\"Rank\":\"Six\"},{\"Suit\":\"Clubs\",\"Rank\":\"Seven\"},{\"Suit\":\"Clubs\",\"Rank\":\"Eight\"},{\"Suit\":\"Clubs\",\"Rank\":\"Nine\"},{\"Suit\":\"Clubs\",\"Rank\":\"Ten\"},{\"Suit\":\"Clubs\",\"Rank\":\"Jack\"},{\"Suit\":\"Clubs\",\"Rank\":\"Queen\"},{\"Suit\":\"Clubs\",\"Rank\":\"King\"},{\"Suit\":\"Clubs\",\"Rank\":\"Ace\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Two\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Three\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Four\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Five\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Six\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Seven\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Eight\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Nine\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Ten\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Jack\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Queen\"},{\"Suit\":\"Diamonds\",\"Rank\":\"King\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Ace\"},{\"Suit\":\"Hearts\",\"Rank\":\"Two\"},{\"Suit\":\"Hearts\",\"Rank\":\"Three\"},{\"Suit\":\"Hearts\",\"Rank\":\"Four\"},{\"Suit\":\"Hearts\",\"Rank\":\"Five\"},{\"Suit\":\"Hearts\",\"Rank\":\"Six\"},{\"Suit\":\"Hearts\",\"Rank\":\"Seven\"},{\"Suit\":\"Hearts\",\"Rank\":\"Eight\"},{\"Suit\":\"Hearts\",\"Rank\":\"Nine\"},{\"Suit\":\"Hearts\",\"Rank\":\"Ten\"},{\"Suit\":\"Hearts\",\"Rank\":\"Jack\"},{\"Suit\":\"Hearts\",\"Rank\":\"Queen\"},{\"Suit\":\"Hearts\",\"Rank\":\"King\"},{\"Suit\":\"Hearts\",\"Rank\":\"Ace\"},{\"Suit\":\"Spades\",\"Rank\":\"Two\"},{\"Suit\":\"Spades\",\"Rank\":\"Three\"},{\"Suit\":\"Spades\",\"Rank\":\"Four\"},{\"Suit\":\"Spades\",\"Rank\":\"Five\"},{\"Suit\":\"Spades\",\"Rank\":\"Six\"},{\"Suit\":\"Spades\",\"Rank\":\"Seven\"},{\"Suit\":\"Spades\",\"Rank\":\"Eight\"},{\"Suit\":\"Spades\",\"Rank\":\"Nine\"},{\"Suit\":\"Spades\",\"Rank\":\"Ten\"},{\"Suit\":\"Spades\",\"Rank\":\"Jack\"},{\"Suit\":\"Spades\",\"Rank\":\"Queen\"},{\"Suit\":\"Spades\",\"Rank\":\"King\"},{\"Suit\":\"Spades\",\"Rank\":\"Ace\"}]";
			//json = "[{\"Suit\":\"Spades\",\"Rank\":\"Seven\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Five\"},{\"Suit\":\"Spades\",\"Rank\":\"Queen\"},{\"Suit\":\"Hearts\",\"Rank\":\"Seven\"},{\"Suit\":\"Hearts\",\"Rank\":\"Queen\"},{\"Suit\":\"Spades\",\"Rank\":\"Ace\"},{\"Suit\":\"Spades\",\"Rank\":\"Jack\"},{\"Suit\":\"Clubs\",\"Rank\":\"Ace\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Eight\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Queen\"},{\"Suit\":\"Hearts\",\"Rank\":\"Ace\"},{\"Suit\":\"Clubs\",\"Rank\":\"Four\"},{\"Suit\":\"Spades\",\"Rank\":\"Nine\"},{\"Suit\":\"Hearts\",\"Rank\":\"King\"},{\"Suit\":\"Clubs\",\"Rank\":\"Jack\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Ten\"},{\"Suit\":\"Hearts\",\"Rank\":\"Eight\"},{\"Suit\":\"Hearts\",\"Rank\":\"Jack\"},{\"Suit\":\"Hearts\",\"Rank\":\"Nine\"},{\"Suit\":\"Clubs\",\"Rank\":\"Five\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Jack\"},{\"Suit\":\"Hearts\",\"Rank\":\"Six\"},{\"Suit\":\"Clubs\",\"Rank\":\"Ten\"},{\"Suit\":\"Spades\",\"Rank\":\"Three\"},{\"Suit\":\"Clubs\",\"Rank\":\"Queen\"},{\"Suit\":\"Clubs\",\"Rank\":\"Nine\"},{\"Suit\":\"Clubs\",\"Rank\":\"Seven\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Three\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Nine\"},{\"Suit\":\"Spades\",\"Rank\":\"King\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Two\"},{\"Suit\":\"Clubs\",\"Rank\":\"Two\"},{\"Suit\":\"Spades\",\"Rank\":\"Four\"},{\"Suit\":\"Spades\",\"Rank\":\"Five\"},{\"Suit\":\"Clubs\",\"Rank\":\"Six\"},{\"Suit\":\"Clubs\",\"Rank\":\"Three\"},{\"Suit\":\"Hearts\",\"Rank\":\"Two\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Four\"},{\"Suit\":\"Hearts\",\"Rank\":\"Three\"},{\"Suit\":\"Diamonds\",\"Rank\":\"King\"},{\"Suit\":\"Spades\",\"Rank\":\"Ten\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Ace\"},{\"Suit\":\"Hearts\",\"Rank\":\"Five\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Six\"},{\"Suit\":\"Clubs\",\"Rank\":\"King\"},{\"Suit\":\"Spades\",\"Rank\":\"Six\"},{\"Suit\":\"Hearts\",\"Rank\":\"Ten\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Seven\"},{\"Suit\":\"Spades\",\"Rank\":\"Two\"},{\"Suit\":\"Spades\",\"Rank\":\"Eight\"},{\"Suit\":\"Clubs\",\"Rank\":\"Eight\"},{\"Suit\":\"Hearts\",\"Rank\":\"Four\"}]";
			json = "[{\"Suit\":\"Diamonds\",\"Rank\":\"Five\"},{\"Suit\":\"Clubs\",\"Rank\":\"Three\"},{\"Suit\":\"Clubs\",\"Rank\":\"Ten\"},{\"Suit\":\"Clubs\",\"Rank\":\"Jack\"},{\"Suit\":\"Hearts\",\"Rank\":\"Seven\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Two\"},{\"Suit\":\"Clubs\",\"Rank\":\"Five\"},{\"Suit\":\"Hearts\",\"Rank\":\"King\"},{\"Suit\":\"Hearts\",\"Rank\":\"Ten\"},{\"Suit\":\"Hearts\",\"Rank\":\"Ace\"},{\"Suit\":\"Diamonds\",\"Rank\":\"King\"},{\"Suit\":\"Spades\",\"Rank\":\"Ten\"},{\"Suit\":\"Clubs\",\"Rank\":\"Nine\"},{\"Suit\":\"Hearts\",\"Rank\":\"Three\"},{\"Suit\":\"Hearts\",\"Rank\":\"Nine\"},{\"Suit\":\"Spades\",\"Rank\":\"Seven\"},{\"Suit\":\"Spades\",\"Rank\":\"Nine\"},{\"Suit\":\"Clubs\",\"Rank\":\"Six\"},{\"Suit\":\"Clubs\",\"Rank\":\"Eight\"},{\"Suit\":\"Hearts\",\"Rank\":\"Four\"},{\"Suit\":\"Hearts\",\"Rank\":\"Two\"},{\"Suit\":\"Spades\",\"Rank\":\"Four\"},{\"Suit\":\"Spades\",\"Rank\":\"Five\"},{\"Suit\":\"Spades\",\"Rank\":\"Six\"},{\"Suit\":\"Spades\",\"Rank\":\"Ace\"},{\"Suit\":\"Hearts\",\"Rank\":\"Eight\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Nine\"},{\"Suit\":\"Clubs\",\"Rank\":\"Two\"},{\"Suit\":\"Clubs\",\"Rank\":\"King\"},{\"Suit\":\"Clubs\",\"Rank\":\"Queen\"},{\"Suit\":\"Clubs\",\"Rank\":\"Seven\"},{\"Suit\":\"Spades\",\"Rank\":\"Three\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Jack\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Ace\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Queen\"},{\"Suit\":\"Hearts\",\"Rank\":\"Jack\"},{\"Suit\":\"Spades\",\"Rank\":\"King\"},{\"Suit\":\"Hearts\",\"Rank\":\"Five\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Three\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Eight\"},{\"Suit\":\"Spades\",\"Rank\":\"Queen\"},{\"Suit\":\"Hearts\",\"Rank\":\"Six\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Four\"},{\"Suit\":\"Spades\",\"Rank\":\"Two\"},{\"Suit\":\"Clubs\",\"Rank\":\"Ace\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Ten\"},{\"Suit\":\"Spades\",\"Rank\":\"Jack\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Six\"},{\"Suit\":\"Hearts\",\"Rank\":\"Queen\"},{\"Suit\":\"Spades\",\"Rank\":\"Eight\"},{\"Suit\":\"Diamonds\",\"Rank\":\"Seven\"},{\"Suit\":\"Clubs\",\"Rank\":\"Four\"}]";

			var cards = JsonConvert.DeserializeObject<PlayingCard[]>(json, settings);
			deck = new PlayingCardDeck(cards);

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
