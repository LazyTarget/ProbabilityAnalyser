using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests.Strategy
{
	[TestFixture]
	public abstract class AcesUpStrategySpecification : SpecificationBase
	{
		protected AcesUpRunContext Context;
		protected ICardMovingStrategy MovingStrategy;


		protected override void Setup()
		{
			base.Setup();
			given();
			when();
		}

		protected virtual void given()
		{
			var deck = new PlayingCardDeck(new PlayingCard[0]);
			Context = new AcesUpRunContext(deck, CancellationToken.None);
			Context.Prioritizer = new DefaultPilePrioritizer();
		}

		protected virtual void when()
		{
		}



		protected virtual void HasTopCards(AcesUpFaceUpCards actual, params PlayingCard[] expectedTop)
		{
			AssertCard(expectedTop[0], actual.Pile1.LastOrDefault());
			AssertCard(expectedTop[1], actual.Pile2.LastOrDefault());
			AssertCard(expectedTop[2], actual.Pile3.LastOrDefault());
			AssertCard(expectedTop[3], actual.Pile4.LastOrDefault());
		}

		protected virtual void AssertCard(PlayingCard expected, PlayingCard actual)
		{
			if (expected == null)
			{
				Assert.IsNull(actual, $"Expected empty pile, card was: {actual}");
				return;
			}
			else
				Assert.IsNotNull(actual, $"Expected NON-empty pile");

			Assert.AreEqual(expected.Suit, actual.Suit, $"Expected: {expected}, Actual: {actual}");
			Assert.AreEqual(expected.Rank, actual.Rank, $"Expected: {expected}, Actual: {actual}");
		}
	}
}
