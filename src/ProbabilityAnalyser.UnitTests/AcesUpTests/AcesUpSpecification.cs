using System;
using System.Collections.Generic;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests
{
	[TestFixture]
	public class AcesUpSpecification : SpecificationBase
	{
		protected readonly IList<Action<AcesUpRunContext>> Configurations
			= new List<Action<AcesUpRunContext>>();


		protected int Instances;
		protected object Result;


		protected override void Setup()
		{
			Instances = AcesUpTests.NR_OF_INSTANCES;

			base.Setup();
			given();
			when();
		}

		protected virtual void given()
		{

		}

		protected virtual void when()
		{
			Action<AcesUpRunContext> configure;
			configure = (ctx) =>
			{
				foreach (var c in Configurations)
				{
					c(ctx);
				}
			};

			int wins = AcesUpTests.RunTest(configure);
			Result = wins;
		}

		public class count_wins : AcesUpSpecification
		{
			protected virtual void BetterThan(double percentage)
			{
				var actual = (int)(Result ?? 0) / (double)Instances;
				Assert.IsTrue(actual > percentage, $"Did not reach target limit of {percentage:P2}, was: {actual:P2}");
			}
		}

		public class strategy_is_move_first_available_to_empty : count_wins
		{
			protected override void given()
			{
				base.given();
				Configurations.Add(c =>
				{
					c.MovingStrategy = new MoveFirstAvailableCardToEmptySpace();
				});
			}


			[Test]
			public void should_have_more_than_0_5_percent_win_rate()
			{
				var expected = 0.005;
				BetterThan(expected);
			}
		}

		public class strategy_is_move_based_on_card_under_top_card : count_wins
		{
			protected override void given()
			{
				base.given();
				Configurations.Add(c =>
				{
					c.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard();
				});
			}


			[Test]
			public virtual void should_have_more_than_0_5_percent_win_rate()
			{
				var expected = 0.005;
				BetterThan(expected);
			}

			[Test]
			public virtual void have_at_least_one_win()
			{
				var actual = (int)(Result ?? 0);
				Assert.IsTrue(actual > 1, $"Has no wins on {Instances} runs!");
			}



			public class strategy_is_move_first_available_to_empty : count_wins.strategy_is_move_based_on_card_under_top_card
			{
				protected override void given()
				{
					base.given();
					Configurations.Add(c =>
					{
						c.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(c.MovingStrategy);
					});
				}


				//[Test]
				//public override void should_have_more_than_0_5_percent_win_rate()
				//{
				//	var expected = 0.005;
				//	BetterThan(expected);
				//}
			}
		}

	}
}
