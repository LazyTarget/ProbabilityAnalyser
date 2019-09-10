using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.UnitTests
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
			[Test]
			public void should_have_more_than_0_5_percent_win_rate()
			{
				var expected = 0.005;
				var percent = (double) Result / (double) Instances;
				Assert.IsTrue(percent > expected);
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
			}
		}

	}
}
