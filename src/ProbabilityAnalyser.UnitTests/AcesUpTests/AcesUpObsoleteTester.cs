using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;
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
			TextWriter writer = null;
			Func<string, string> logFormatter = null;

			var deck = PlayingCardDeck.Standard52CardDeck();


			var oldCtx = new AcesUpOld.AcesUpRunContext(new PlayingCardDeck(deck.Cards), CancellationToken.None);
			oldCtx.MovingStrategy2 = new AcesUpOld.MoveCardBasedOnDirectlyUnderTopCard(new AcesUpOld.MoveFirstAvailableCardToEmptySpace());

			var newCtx = new AcesUpRunContext(new PlayingCardDeck(deck.Cards), CancellationToken.None);
			newCtx.MovingStrategy = new MoveCardBasedOnDirectlyUnderTopCard(new MoveFirstAvailableCardToEmptySpace());

			var old = new AcesUpOld(writer, logFormatter);
			var acesUp = new AcesUp(writer, logFormatter);

			var oldPts = old.Run(oldCtx);

			var newPts = acesUp.Run(newCtx);

			Console.WriteLine($"Old: {oldPts}");
			Console.WriteLine($"New: {newPts}");

			Assert.IsTrue(newPts >= oldPts, $"Old program gave better results: {oldPts} vs. {newPts}");
		}
	}
}
