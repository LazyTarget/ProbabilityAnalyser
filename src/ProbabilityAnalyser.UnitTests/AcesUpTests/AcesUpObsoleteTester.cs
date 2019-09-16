﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
			double times = 100;
			Func<string, string> logFormatter = null;

			double oldBetter = 0;
			double oldWins = 0;
			double newWins = 0;
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

				//var logDetails = oldPts > newPts;
				var logDetails = oldPts == 100;
				if (logDetails)
				{
					oldWins++;

					// Old gave better results...
					if (oldPts > newPts)
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

					var settings = new JsonSerializerSettings();
					settings.Formatting = Formatting.None;
					settings.Converters.Add(new StringEnumConverter());
					var json = JsonConvert.SerializeObject(deck.Cards, settings);
					Console.WriteLine($"Deck: {json}");

					Console.WriteLine("--- --- --- ---");
				}
				else if (newPts == 100)
				{
					newWins++;
					Console.WriteLine("New won but Old didn't!");
				}
			};

			CommonExtensions.InvokeLoop(false, (int)times, loop);


			Console.WriteLine($"Old {oldWins} wins times; {(oldWins / times):P2}");
			Console.WriteLine($"Old was better {oldBetter} times; {(oldBetter / times):P2}");

			Console.WriteLine($"New {newWins} wins times; {(newWins / times):P2}");

			if (oldWins <= 0)
				Assert.Inconclusive($"Old gave no wins, so cannot compare strategies");

			Assert.AreEqual(0, (int)oldBetter, $"Old program gave better results {oldBetter} times, should be 0 times");
		}
	}
}
