using System;
using System.Collections.Generic;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Helpers;
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
			var pcb = new PlayingCardBuilder();
			var cards = Context.FaceUpCards;

			// ♧5      ♡A  ♡4 
			//             ♧3 
			cards.Pile1.Pile = new PlayingCard[] { pcb.Clubs(PlayingCardRank.Five)};
			cards.Pile2.Pile = new PlayingCard[] {};
			cards.Pile3.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Ace), };
			cards.Pile4.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Four), pcb.Clubs(PlayingCardRank.Three), };

			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				null,
				pcb.Hearts(PlayingCardRank.Ace),
				pcb.Clubs(PlayingCardRank.Three)
			);

			var m = MovingStrategy.MoveCard(Context);

			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Clubs(PlayingCardRank.Three),
				pcb.Hearts(PlayingCardRank.Ace),
				pcb.Hearts(PlayingCardRank.Four)
			);
		}
	}
}
