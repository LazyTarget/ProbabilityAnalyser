using System;
using System.Collections.Generic;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Helpers;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Prioritizer;
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
		public void when_no_empty_piles()
		{
			bool m;
			var pcb = new PlayingCardBuilder();
			var cards = Context.FaceUpCards;
			cards.Pile1.Pile = new PlayingCard[] { pcb.Clubs(PlayingCardRank.Five), };
			cards.Pile2.Pile = new PlayingCard[] { pcb.Clubs(PlayingCardRank.Three), };
			cards.Pile3.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Ace), };
			cards.Pile4.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Four), };

			// ♧5  ♧3  ♡A  ♡4 
			//              
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Clubs(PlayingCardRank.Three),
				pcb.Hearts(PlayingCardRank.Ace),
				pcb.Hearts(PlayingCardRank.Four)
			);

			m = MovingStrategy.MoveCard(Context);

			// ♧5  ♧3  ♡A  ♡4 
			//                
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Clubs(PlayingCardRank.Three),
				pcb.Hearts(PlayingCardRank.Ace),
				pcb.Hearts(PlayingCardRank.Four)
			);

			// No moves should have been made!
			Assert.IsFalse(m, $"Should not have made any moves! Top cards: {cards}");
		}


		[Test]
		public void when_one_empty_and_one_possible_pile()
		{
			bool m;
			var pcb = new PlayingCardBuilder();
			var cards = Context.FaceUpCards;
			cards.Pile1.Pile = new PlayingCard[] { pcb.Clubs(PlayingCardRank.Five)};
			cards.Pile2.Pile = new PlayingCard[] {};
			cards.Pile3.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Ace), };
			cards.Pile4.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Four), pcb.Clubs(PlayingCardRank.Three), };

			// ♧5      ♡A  ♡4 
			//             ♧3 
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				null,
				pcb.Hearts(PlayingCardRank.Ace),
				pcb.Clubs(PlayingCardRank.Three)
			);

			m = MovingStrategy.MoveCard(Context);

			// ♧5  ♧3  ♡A  ♡4 
			//                
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Clubs(PlayingCardRank.Three),
				pcb.Hearts(PlayingCardRank.Ace),
				pcb.Hearts(PlayingCardRank.Four)
			);

			// Has no more moves...
			m = MovingStrategy.MoveCard(Context);
			Assert.IsFalse(m, $"Should not have any more moves to do; Top cards: {cards}");
		}


		[Test]
		public void when_two_empty_and_one_possible_pile()
		{
			bool m;
			var pcb = new PlayingCardBuilder();
			var cards = Context.FaceUpCards;
			cards.Pile1.Pile = new PlayingCard[] { pcb.Clubs(PlayingCardRank.Five) };
			cards.Pile2.Pile = new PlayingCard[] { };
			cards.Pile3.Pile = new PlayingCard[] { };
			cards.Pile4.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Four), pcb.Clubs(PlayingCardRank.Three), };

			// ♧5          ♡4 
			//             ♧3 
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				null,
				null,
				pcb.Clubs(PlayingCardRank.Three)
			);

			m = MovingStrategy.MoveCard(Context);

			// ♧5  ♧3      ♡4 
			//                
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Clubs(PlayingCardRank.Three),
				null,
				pcb.Hearts(PlayingCardRank.Four)
			);

			// Has no more moves...
			m = MovingStrategy.MoveCard(Context);
			Assert.IsFalse(m, $"Should not have any more moves to do; Top cards: {cards}");
		}


		[Test]
		public void when_two_empty_and_one_possible_piles()
		{
			bool m;
			var pcb = new PlayingCardBuilder();
			var cards = Context.FaceUpCards;
			cards.Pile1.Pile = new PlayingCard[] { pcb.Clubs(PlayingCardRank.Five) };
			cards.Pile2.Pile = new PlayingCard[] { };
			cards.Pile3.Pile = new PlayingCard[] { };
			cards.Pile4.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Four), pcb.Clubs(PlayingCardRank.Three), };

			// ♧5          ♡4 
			//             ♧3 
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				null,
				null,
				pcb.Clubs(PlayingCardRank.Three)
			);

			m = MovingStrategy.MoveCard(Context);

			// ♧5  ♧3      ♡4 
			//                
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Clubs(PlayingCardRank.Three),
				null,
				pcb.Hearts(PlayingCardRank.Four)
			);

			// Has no more moves...
			m = MovingStrategy.MoveCard(Context);
			Assert.IsFalse(m, $"Should not have any more moves to do; Top cards: {cards}");
		}


		[Test]
		public void when_two_empty_and_two_possible_piles()
		{
			bool m;
			var pcb = new PlayingCardBuilder();
			var cards = Context.FaceUpCards;
			cards.Pile1.Pile = new PlayingCard[] { pcb.Clubs(PlayingCardRank.Five), pcb.Hearts(PlayingCardRank.Ace) };
			cards.Pile2.Pile = new PlayingCard[] { };
			cards.Pile3.Pile = new PlayingCard[] { };
			cards.Pile4.Pile = new PlayingCard[] { pcb.Hearts(PlayingCardRank.Four), pcb.Clubs(PlayingCardRank.Three), };

			// ♧5          ♡4 
			// ♡A          ♧3 
			HasTopCards(cards,
				pcb.Hearts(PlayingCardRank.Ace),
				null,
				null,
				pcb.Clubs(PlayingCardRank.Three)
			);

			m = MovingStrategy.MoveCard(Context);

			// ♧5  ♡A      ♡4 
			//             ♧3 
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Hearts(PlayingCardRank.Ace),
				null,
				pcb.Clubs(PlayingCardRank.Three)
			);

			m = MovingStrategy.MoveCard(Context);

			// ♧5  ♡A  ♧3  ♡4 
			//                
			HasTopCards(cards,
				pcb.Clubs(PlayingCardRank.Five),
				pcb.Hearts(PlayingCardRank.Ace),
				pcb.Clubs(PlayingCardRank.Three),
				pcb.Hearts(PlayingCardRank.Four)
			);

			// Has no more moves...
			m = MovingStrategy.MoveCard(Context);
			Assert.IsFalse(m, $"Should not have any more moves to do; Top cards: {cards}");
		}

	}
}
