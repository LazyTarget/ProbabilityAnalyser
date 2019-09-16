using System;
using System.Collections.Generic;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests.Strategy
{
	[TestFixture]
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
