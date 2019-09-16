using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests
{
	[TestFixture]
	public class AcesUpObsoleteTester : TestBase
	{
		[Test]
		public void Test()
		{
			var times = 10;
			Func<string, string> logFormatter = null;

			int oldBetter = 0;
			Action<int, ParallelLoopState> loop;
			loop = (i, p) =>
			{
				var sb = new StringBuilder(); 
				TextWriter writer = new StringWriter(sb);


				var sb2 = new StringBuilder();
				TextWriter writer2 = new StringWriter(sb2);


				var deck = PlayingCardDeck.Standard52CardDeck();

				var oldCtx = new AcesUpOld.AcesUpRunContext(new PlayingCardDeck(deck.Cards), CancellationToken.None);
				oldCtx.MovingStrategy2 = new AcesUpOld.MoveCardBasedOnDirectlyUnderTopCard(new AcesUpOld.MoveFirstAvailableCardToEmptySpace());

				var newCtx = new AcesUpRunContext(new PlayingCardDeck(deck.Cards), CancellationToken.None);
				newCtx.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(new MoveFirstAvailableCardToEmptySpace());

				var old = new AcesUpOld(writer, logFormatter);
				var acesUp = new AcesUp(writer2, logFormatter);

				var oldPts = old.Run(oldCtx);

				var newPts = acesUp.Run(newCtx);

				if (oldPts > newPts)
				{
					// Old gave better results...
					oldBetter++;

					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine($"[{i}] Output OLD:");
					Console.WriteLine(sb.ToString());

					Console.WriteLine("----");
					Console.WriteLine();
					Console.WriteLine($"[{i}] Output NEW:");
					Console.WriteLine(sb2.ToString());

					Console.WriteLine($"[{i}] Old: {oldPts}");
					Console.WriteLine($"[{i}] New: {newPts}");

					Console.WriteLine("--- --- --- ---");
				}
			};

			CommonExtensions.InvokeLoop(false, times, loop);


			Console.WriteLine($"Old was better {oldBetter} times; {(oldBetter / times):P2}");

			Assert.AreEqual(0, oldBetter, $"Old program gave better results {oldBetter} times, should be 0 times");
		}
	}
}
