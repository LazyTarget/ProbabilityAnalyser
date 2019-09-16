using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests.Strategy
{
	[TestFixture]
	public class AcesUpStrategySpecification : SpecificationBase
	{
		protected AcesUpRunContext Context;
		protected ICardMovingStrategy MovingStrategy;


		protected override void Setup()
		{
			base.Setup();
			given();
			when();
		}

		protected virtual void SetupFaceUpCards(AcesUpFaceUpCards cards)
		{
			cards.Pile1.Pile = new PlayingCard[] { new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Five)};
			cards.Pile2.Pile = new PlayingCard[0];
			cards.Pile3.Pile = new PlayingCard[] { new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Ace), };
			cards.Pile4.Pile = new PlayingCard[] { new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Four), new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Three),  };

			// ♧5      ♡A  ♡4
			//             ♧A
		}

		protected virtual void given()
		{
			var deck = new PlayingCardDeck(new PlayingCard[0]);
			Context = new AcesUpRunContext(deck, CancellationToken.None);
			SetupFaceUpCards(Context.FaceUpCards);
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
				Assert.IsNull(actual);
				return;
			}
			else
				Assert.IsNotNull(actual);

			Assert.AreEqual(expected.Suit, actual.Suit);
			Assert.AreEqual(expected.Rank, actual.Rank);
		}


		public class strategy_is_move_first_available_to_empty : AcesUpStrategySpecification
		{
			protected override void given()
			{
				base.given();

				MovingStrategy = new MoveFirstAvailableCardToEmptySpace(MovingStrategy);
			}


			[Test]
			public void move()
			{
				// ♧5      ♡A  ♡4
				//             ♧A

				HasTopCards(Context.FaceUpCards,
					new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Five),
					new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Four),
					new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Five),
					new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Ace)
				);

				var m = MovingStrategy.MoveCard(Context);

				HasTopCards(Context.FaceUpCards, 
					new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Five),
					null,
					new PlayingCard(PlayingCardSuit.Hearts, PlayingCardRank.Five),
					new PlayingCard(PlayingCardSuit.Clubs, PlayingCardRank.Ace)
				);
			}
		}
	}
}
