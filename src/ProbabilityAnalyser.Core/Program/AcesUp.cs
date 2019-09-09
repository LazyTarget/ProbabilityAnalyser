using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program
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
		

		public class AcesUpRunContext
		{
			public AcesUpRunContext(PlayingCardDeck deck, CancellationToken cancellationToken, bool hardMode = false)
			{
				Deck = deck;
				Token = cancellationToken;
				HardMode = hardMode;
				MovingStrategy =
					new MoveCardBasedOnDirectlyUnderTopCard(
						new MoveFirstAvailableCardToEmptySpace()
					);
			}

			public PlayingCardDeck Deck { get; }
			public AcesUpFaceUpCards FaceUpCards { get; } = new AcesUpFaceUpCards();
			public CancellationToken Token { get; }
			public bool HardMode { get; set; }

			public ICardMovingStrategy MovingStrategy;
		}

		public class AcesUpFaceUpCards
		{
			public int Length => Pile1.Length + Pile2.Length + Pile3.Length + Pile4.Length;

			public PlayingCard[] Pile1 = new PlayingCard[0];
			public PlayingCard[] Pile2 = new PlayingCard[0];
			public PlayingCard[] Pile3 = new PlayingCard[0];
			public PlayingCard[] Pile4 = new PlayingCard[0];

			public IEnumerable<PlayingCard> Top()
			{
				if (Pile1.Any())
					yield return Pile1.LastOrDefault();
				if (Pile2.Any())
					yield return Pile2.LastOrDefault();
				if (Pile3.Any())
					yield return Pile3.LastOrDefault();
				if (Pile4.Any())
					yield return Pile4.LastOrDefault();
			}


			public bool Discard(PlayingCard card)
			{
				if (Top().Contains(card))
				{
					// Card is on top of pile, can discard

					if (Pile1.LastOrDefault() == card)
						Pile1 = Pile1.PopTopCard();
					else if (Pile2.LastOrDefault() == card)
						Pile2 = Pile2.PopTopCard();
					else if (Pile3.LastOrDefault() == card)
						Pile3 = Pile3.PopTopCard();
					else if (Pile4.LastOrDefault() == card)
						Pile4 = Pile4.PopTopCard();
					else
					{
						return false;
					}
					//Console.WriteLine($"Discarded \"{card}\"");
					return true;
				}
				else
				{
					Console.WriteLine($"Cannot discard non top-card: {card}");
				}
				return false;
			}


			public void AppendOneToEmptyPile(PlayingCard card)
			{
				if (Pile1.Length <= 0)
					AppendCardToPile(card, 1);
				else if (!Pile2.Any())
					AppendCardToPile(card, 2);
				else if (!Pile3.Any())
					AppendCardToPile(card, 3);
				else if (!Pile4.Any())
					AppendCardToPile(card, 4);
			}

			public void AppendCardToPile(PlayingCard card, int pile)
			{
				if (pile == 1)
					Pile1 = Pile1.Concat(new[] { card }).ToArray();
				else if (pile == 2)
					Pile2 = Pile2.Concat(new[] { card }).ToArray();
				else if (pile == 3)
					Pile3 = Pile3.Concat(new[] { card }).ToArray();
				else if (pile == 4)
					Pile4 = Pile4.Concat(new[] { card }).ToArray();
				else
				{

				}
			}

			public void AppendCardsToPiles(IEnumerable<PlayingCard> enumerable)
			{
				var cards = enumerable.ToList();
				while (cards.Any())
				{
					for (var p = 1; p <= 4; p++)
					{
						var card = cards.FirstOrDefault();
						if (card == null)
							break;
						AppendCardToPile(card, p);
						cards.RemoveAt(0);
					}
				}
			}
		}


		public interface ICardMovingStrategy
		{
			bool MoveCard(AcesUpRunContext context);
		}


		public class MoveFirstAvailableCardToEmptySpace : ICardMovingStrategy
		{
			private readonly ICardMovingStrategy _fallback;

			public MoveFirstAvailableCardToEmptySpace(ICardMovingStrategy fallback = null)
			{
				_fallback = fallback;
			}

			public virtual bool MoveCard(AcesUpRunContext context)
			{
				PlayingCard peek;
				PlayingCard card;
				var hardMode = context.HardMode;
				var cards = context.FaceUpCards;

				// Get card...;
				if (cards.Pile1.Length > 1 && (peek = TryPopCardFromPile(ref cards.Pile1, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile2.Length > 1 && (peek = TryPopCardFromPile(ref cards.Pile2, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile3.Length > 1 && (peek = TryPopCardFromPile(ref cards.Pile3, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile4.Length > 1 && (peek = TryPopCardFromPile(ref cards.Pile4, hardMode)) != null)
				{
					card = peek;
				}
				else
				{
					// no piles have any cards available to move...

					var moved = false;
					if (_fallback != null)
						moved = _fallback.MoveCard(context);
					return moved;
				}

				// Move card...
				context.FaceUpCards.AppendOneToEmptyPile(card);

				return true;
			}
		}

		public class MoveCardBasedOnDirectlyUnderTopCard : ICardMovingStrategy
		{
			private readonly ICardMovingStrategy _fallback;

			public MoveCardBasedOnDirectlyUnderTopCard(ICardMovingStrategy fallback = null)
			{
				_fallback = fallback;
			}

			public virtual bool MoveCard(AcesUpRunContext context)
			{
				// todo: Implement "AI" which remembers the card under the Top card(s), for better 'moving-strategy'

				var top = context.FaceUpCards.Top().ToArray();
				if (top.Length >= 4)
					return false;       // no change

				var suitsOnTop = top.Select(c => c.Suit).Distinct().ToArray();

				PlayingCard card;
				PlayingCard peek;
				var hardMode = context.HardMode;
				var cards = context.FaceUpCards;

				if (cards.Pile1.Length > 1 && (peek = cards.Pile1.Reverse().ElementAtOrDefault(1)) != null && suitsOnTop.Contains(peek.Suit) &&
					(peek = TryPopCardFromPile(ref cards.Pile1, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile2.Length > 1 && (peek = cards.Pile2.Reverse().ElementAtOrDefault(1)) != null && suitsOnTop.Contains(peek.Suit) &&
						 (peek = TryPopCardFromPile(ref cards.Pile2, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile3.Length > 1 && (peek = cards.Pile3.Reverse().ElementAtOrDefault(1)) != null && suitsOnTop.Contains(peek.Suit) &&
						 (peek = TryPopCardFromPile(ref cards.Pile3, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile4.Length > 1 && (peek = cards.Pile4.Reverse().ElementAtOrDefault(1)) != null && suitsOnTop.Contains(peek.Suit) &&
						 (peek = TryPopCardFromPile(ref cards.Pile4, hardMode)) != null)
				{
					card = peek;
				}
				else
				{
					var moved = false;
					if (_fallback != null)
						moved = _fallback.MoveCard(context);
					return moved;
				}


				// Move card...
				context.FaceUpCards.AppendOneToEmptyPile(card);

				return true;
			}
		}

		public class AcesToEmptyPiles : ICardMovingStrategy
		{
			private readonly ICardMovingStrategy _fallback;

			public AcesToEmptyPiles(ICardMovingStrategy fallback = null)
			{
				_fallback = fallback;
			}

			public virtual bool MoveCard(AcesUpRunContext context)
			{
				var top = context.FaceUpCards.Top().ToArray();
				if (top.Length >= 4)
					return false;       // has no empty piles...

				bool moved;
				PlayingCard peek;
				PlayingCard card;
				var hardMode = context.HardMode;
				var cards = context.FaceUpCards;

				// Get card...;
				if (cards.Pile1.Length > 1 && (peek = cards.Pile1.Last()) != null && peek.Rank == PlayingCardRank.Ace &&
				    (peek = TryPopCardFromPile(ref cards.Pile1, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile2.Length > 1 && (peek = cards.Pile2.Last()) != null && peek.Rank == PlayingCardRank.Ace &&
				         (peek = TryPopCardFromPile(ref cards.Pile2, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile3.Length > 1 && (peek = cards.Pile3.Last()) != null && peek.Rank == PlayingCardRank.Ace &&
				         (peek = TryPopCardFromPile(ref cards.Pile3, hardMode)) != null)
				{
					card = peek;
				}
				else if (cards.Pile4.Length > 1 && (peek = cards.Pile4.Last()) != null && peek.Rank == PlayingCardRank.Ace &&
				         (peek = TryPopCardFromPile(ref cards.Pile4, hardMode)) != null)
				{
					card = peek;
				}
				else
				{
					// no piles have any cards available to move...

					moved = false;
					if (_fallback != null)
						moved = _fallback.MoveCard(context);
					return moved;
				}

				// Move card...
				context.FaceUpCards.AppendOneToEmptyPile(card);
				moved = true;

				return moved;
			}
		}

	}
}
