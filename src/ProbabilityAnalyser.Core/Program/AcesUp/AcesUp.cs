using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp
{
	public class AcesUp
	{
		private readonly TextWriter _output;
		private readonly Func<string, string> _logFormatter;

		public AcesUp(TextWriter output, Func<string, string> logFormatter = null)
		{
			_output = output;
			_logFormatter = logFormatter;
		}

		public int Run(PlayingCardDeck deck)
		{
			var result = Run(deck, CancellationToken.None);
			return result;
		}
		public int Run(PlayingCardDeck deck, CancellationToken cancellationToken)
		{
			var context = new AcesUpRunContext(deck, cancellationToken);
			var result = Run(context);
			return result;
		}

		public int Run(AcesUpRunContext context)
		{
			/*
				1. Deal four cards in a row face up.
				2. If there are two or more cards of the same suit, discard all but the highest-ranked card of that suit. Aces rank high.
				3. Repeat step 2 until there are no more pairs of cards with the same suit.
				4. Whenever there are any empty spaces, you may choose the top card of another pile to be put into the empty space. After you do this, go to Step 2.
				5. When there are no more cards to move or remove, deal out the next four cards from the deck face-up onto each pile.
				6. Repeat Step 2, using only the visible, or top, cards on each of the four piles.
				7. When the last four cards have been dealt out and any moves made, the game is over. The fewer cards left in the tableau, the better. To win is to have only the four aces left.
				
				When the game ends, the number of discarded cards is your score. The maximum score (and thus the score necessary to win) is 48, which means all cards have been discarded except for the four aces, thus the name of the game.
			*/




			int loops = 0;
			bool gameover = false;
			while (!gameover)
			{
				gameover = !DealAndCheck(context);
				loops++;

				PrintContext(context, "loop");
			}


			// No more cards to deal to piles, GAMEOVER!
			var points = 52 - context.FaceUpCards.Length;
			if (points == 48)
			{
				if (context.FaceUpCards.Top().Count(x => x.Rank == PlayingCardRank.Ace) == 4)
					points = 100;
			}

			return points;
		}

		private void PrintContext(AcesUpRunContext context, string extra)
		{
			if (_output == null)
				return;

			var str = "";

			var card = context.FaceUpCards.Pile1.LastOrDefault();
			if (card != null)
				str += $"{card?.ToShortString()} ";
			else
				str += $"    ";

			card = context.FaceUpCards.Pile2.LastOrDefault();
			if (card != null)
				str += $"{card?.ToShortString()} ";
			else
				str += $"    ";

			card = context.FaceUpCards.Pile3.LastOrDefault();
			if (card != null)
				str += $"{card?.ToShortString()} ";
			else
				str += $"    ";

			card = context.FaceUpCards.Pile4.LastOrDefault();
			if (card != null)
				str += $"{card?.ToShortString()} ";
			else
				str += $"    ";


			str += $"  ({extra})";

			if (_logFormatter != null)
				str = _logFormatter.Invoke(str);

			_output?.WriteLine(str);

			if (System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Trace.WriteLine(str);
		}


		private bool DealAndCheck(AcesUpRunContext context)
		{
			// Pseudo-flow

			if (!Deal4Cards(context))                           // 1
			{
				return false;
			}


			while (CheckAndDiscardSuits(context))               // 2
			{
				if (MoveCardsIfHasEmptySpaces(context))         // 3
				{
					//CheckAndDiscardSuits(context);              // 2
				}
				else
					break;
			}


			return !context.Deck.IsEmpty;
		}

		private bool CheckAndDiscardSuits(AcesUpRunContext context)
		{
			var discarded = false;
			var top = context.FaceUpCards.Top().ToArray();

			// 2. If there are two or more cards of the same suit, discard all but the highest-ranked card of that suit. Aces rank high.
			var groupBySuits = top.GroupBy(x => x.Suit);
			foreach (var group in groupBySuits)
			{
				var suit = group.Key;
				var count = group.Count();
				if (count >= 2)
				{
					var remainingCard = group.AllButHighestCardOfSuit(suit, aceRankHigh: true);

					var removed = group
						.Where(c => c.Suit == suit)
						.Where(c => c != remainingCard)
						.Count(c => context.FaceUpCards.Discard(c));

					if (removed > 0)
					{
						discarded = true;
						break;
					}
				}
			}


			if (discarded)
			{
				PrintContext(context, "discarded");

				// Continue to check for cards to discard...
				var r = CheckAndDiscardSuits(context);
			}

			return discarded;
		}


		private bool MoveCardsIfHasEmptySpaces(AcesUpRunContext context)
		{
			if (context.FaceUpCards.Top().Count() >= 4)
			{
				// No empty pile to move to...
				return false;
			}

			ICardMovingStrategy movingStrategy = context.MovingStrategy;
			if (movingStrategy == null)
			{
				throw new NullReferenceException($"Invalid moving strategy!");
			}

			bool moved;
			bool changed = false;
			do
			{
				moved = movingStrategy.MoveCard(context);
				if (moved)
				{
					changed = true;
					PrintContext(context, "moved");
				}

			} while (moved && context.FaceUpCards.Top().Count() < 4);

			return changed;
		}



		// Utils

		private static PlayingCard TryPopCardFromPile(ref PlayingCard[] pile, bool hardMode)
		{
			PlayingCard card = null;
			if (pile.Length > 1)
			{
				card = pile.Last();
				if (!hardMode || card.Rank == PlayingCardRank.Ace)
					pile = pile.Take(pile.Length - 1).ToArray();
			}
			return card;
		}



		private bool Deal4Cards(AcesUpRunContext context)
		{
			// 1 & 5
			var cards = context.Deck.DrawMany(4);
			context.FaceUpCards.AppendCardsToPiles(cards);


			PrintContext(context, "deal");

			return cards.Length > 0;
		}
	}
}
