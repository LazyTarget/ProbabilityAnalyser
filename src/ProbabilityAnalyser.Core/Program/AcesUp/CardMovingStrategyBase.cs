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


		protected virtual IEnumerable<AcesUpPile> Prioritize(AcesUpRunContext context)
		{
			yield return context.FaceUpCards.Pile1;
			yield return context.FaceUpCards.Pile2;
			yield return context.FaceUpCards.Pile3;
			yield return context.FaceUpCards.Pile4;
		}

		protected abstract bool Peek(AcesUpPile pile);

		protected virtual PlayingCard Pop(AcesUpPile pile)
		{
			PlayingCard card;
			pile.PopTopCard(out card);
			return card;
		}


		public bool MoveCard(AcesUpRunContext context)
		{
			var top = context.FaceUpCards.Top().ToArray();

			if (top.Length >= 4)
				return false;       // has no empty piles...

			if (context.FaceUpCards.Length <= 4)
				return false;		// has no pile with more than 1 card


			PlayingCard card = null;

			var priority = Prioritize(context);
			foreach (var pile in priority)
			{
				if (pile.Length < 2)
					continue;		// pile is to small to move cards from

				if (context.HardMode)
				{
					if (pile.Last().Rank != PlayingCardRank.Ace)
					{
						System.Diagnostics.Debug.WriteLine($"Not allowed to move cards other than '{PlayingCardRank.Ace}'");
						continue;
					}
				}


				if (Peek(pile))
				{
					card = Pop(pile);
				}

				if (card != null)
					break;
			}


			bool moved;
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
