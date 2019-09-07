﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program
{
	public class AcesUp
	{
		public int Run(PlayingCardDeck deck)
		{
			var result = Run(deck, CancellationToken.None);
			return result;
		}

		public int Run(PlayingCardDeck deck, CancellationToken cancellationToken)
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


			var context = new AcesUpRunContext(deck, cancellationToken);


			int loops = 0;
			bool gameover = false;
			while (!gameover)
			{
				gameover = !DealAndCheck(context);
				loops++;
			}


			// No more cards to deal to piles, GAMEOVER!
			var points = 52 - context.FaceUpCards.Length;
			//if (points == 48)
			//{
			//	Console.WriteLine("Perfect game!!!");
			//}
			//else
			//{
			//	Console.WriteLine($"Gameover with {points} points");
			//}

			//// 1. Deal four cards in a row face up.
			//Deal4Cards(context);


			//while (!deck.IsEmpty)
			//{
			//	HandleChecks(context);

			//	Deal4Cards(context);

			//}

			return points;
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

					var removed = group.Count(c =>
					{
						if (c.Suit != suit)
							return false;
						if (c == remainingCard)
							return false;

						var d = context.FaceUpCards.Discard(c);
						return d;
					});


					if (removed > 0)
					{
						return true;

						DrawUpToFourFaceUpCards(context);
						HandleChecks(context);
					}
				}
			}
			return false;
		}
		

		private bool MoveCardsIfHasEmptySpaces(AcesUpRunContext context)
		{
			Func<AcesUpRunContext, bool> movingStrategy;

			// Strategy 1
			movingStrategy = MoveFirstAvailableCardToEmptySpace;

			//// Strategy 2
			//movingStrategy = MoveCardBasedOnCardsUnderTop;		// todo: implement


			//bool moved;
			//bool changed = false;
			//do
			//{
			//	moved = movingStrategy(context);
			//	if (moved)
			//		changed = true;

			//} while (moved);


			var changed = movingStrategy(context);

			return changed;
		}


		private bool MoveFirstAvailableCardToEmptySpace(AcesUpRunContext context)
		{
			PlayingCard card;

			// Get card...
			if (context.FaceUpCards.Pile1.Count() > 1)
				context.FaceUpCards.Pile1 = context.FaceUpCards.Pile1.PopTopCard(out card);
			else if (context.FaceUpCards.Pile2.Count() > 1)
				context.FaceUpCards.Pile2 = context.FaceUpCards.Pile2.PopTopCard(out card);
			else if (context.FaceUpCards.Pile3.Count() > 1)
				context.FaceUpCards.Pile3 = context.FaceUpCards.Pile3.PopTopCard(out card);
			else if (context.FaceUpCards.Pile4.Count() > 1)
				context.FaceUpCards.Pile4 = context.FaceUpCards.Pile4.PopTopCard(out card);
			else
				return false;		// no piles have any cards available to move...

			// Move card...
			context.FaceUpCards.AppendOneToEmptyPile(card);
			return true;
		}

		private bool MoveCardBasedOnCardsUnderTop(AcesUpRunContext context)
		{
			// todo: Implement "AI" which remembers the card under the Top card(s), for better 'moving-strategy'

			throw new NotImplementedException();
		}




		// Utils


		private void HandleChecks(AcesUpRunContext context)
		{
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
					
					var removed = group.Count(c =>
					{
						if (c.Suit != suit)
							return false;
						if (c == remainingCard)
							return false;

						var d = context.FaceUpCards.Discard(c);
						return d;
					});
					Console.WriteLine($"Removed {removed} cards of suit '{suit}'");


					if (removed > 0)
					{
						DrawUpToFourFaceUpCards(context);
						HandleChecks(context);
					}
				}
			}
		}

		private bool Deal4Cards(AcesUpRunContext context)
		{
			// 1 & 5
			var cards = context.Deck.DrawMany(4);
			context.FaceUpCards.AppendCardsToPiles(cards);

			return cards.Length > 0;
		}

		private void DrawUpToFourFaceUpCards(AcesUpRunContext context)
		{
			var faceUpCards = context.FaceUpCards;
			while (faceUpCards.Top().Count() < 4)
			{
				var card = context.Deck.Draw();
				faceUpCards.AppendOneToEmptyPile(card);
			}
		}


		private class AcesUpRunContext
		{
			public AcesUpRunContext(PlayingCardDeck deck, CancellationToken cancellationToken)
			{
				Deck = deck;
				Token = cancellationToken;
			}

			public PlayingCardDeck Deck { get; set; }
			public AcesUpFaceUpCards FaceUpCards { get; set; } = new AcesUpFaceUpCards();
			public CancellationToken Token { get; set; }
		}

		private class AcesUpFaceUpCards
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
					Pile1 = Pile1.Concat(new[] {card}).ToArray();
				else if (pile == 2)
					Pile2 = Pile2.Concat(new[] {card}).ToArray();
				else if (pile == 3)
					Pile3 = Pile3.Concat(new[] {card}).ToArray();
				else if (pile == 4)
					Pile4 = Pile4.Concat(new[] {card}).ToArray();
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
	}
}
