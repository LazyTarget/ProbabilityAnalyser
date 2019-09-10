using System;
using System.Collections.Generic;
using System.Linq;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp
{
	public abstract class CardMovingStrategyBase : ICardMovingStrategy
	{
		private readonly ICardMovingStrategy _fallback;

		protected CardMovingStrategyBase(ICardMovingStrategy fallback = null)
		{
			_fallback = fallback;
		}


		protected virtual IEnumerable<PlayingCard[]> Prioritize(AcesUpFaceUpCards cards)
		{
			yield return cards.Pile1;
			yield return cards.Pile2;
			yield return cards.Pile3;
			yield return cards.Pile4;
		}

		protected abstract bool Peek(PlayingCard[] pile);

		protected virtual PlayingCard Pop(ref PlayingCard[] pile)
		{
			PlayingCard card;
			pile = pile.PopTopCard(out card);
			return card;
		}


		public bool MoveCard(AcesUpRunContext context)
		{
			var top = context.FaceUpCards.Top().ToArray();

			if (top.Length >= 4)
				return false;       // has no empty piles...

			if (context.FaceUpCards.Length <= 4)
				return false;		// has no pile with more than 1 card



			bool moved;
			PlayingCard card = null;
			var hardMode = context.HardMode;
			var cards = context.FaceUpCards;


			var priority = Prioritize(context.FaceUpCards);
			foreach (var pile in priority)
			{
				if (pile.Length < 2)
					continue;		// pile is to small to move cards from

				if (hardMode)
				{
					if (pile.Last().Rank != PlayingCardRank.Ace)
					{
						System.Diagnostics.Debug.WriteLine($"Not allowed to move cards other than '{PlayingCardRank.Ace}'");
						continue;
					}
				}

				if (Peek(pile))
				{
					if (pile == cards.Pile1)
						card = Pop(ref context.FaceUpCards.Pile1);
					else if (pile == cards.Pile2)
						card = Pop(ref context.FaceUpCards.Pile2);
					else if (pile == cards.Pile3)
						card = Pop(ref context.FaceUpCards.Pile3);
					else if (pile == cards.Pile4)
						card = Pop(ref context.FaceUpCards.Pile4);
				}
				if (card != null)
					break;
			}


			if (card != null)
			{
				// Move card...
				moved = context.FaceUpCards.AppendOneToEmptyPile(card);
				if (!moved)
				{
					throw new Exception($"Card was popped from a pile but could not be appended to an empty pile!");
				}
			}
			else
			{
				// no piles have any cards available to move...

				moved = false;
				if (_fallback != null)
					moved = _fallback.MoveCard(context);
			}
			return moved;

		}
	}
}
