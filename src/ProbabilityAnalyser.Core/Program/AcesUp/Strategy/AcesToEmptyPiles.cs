using System;
using System.Collections.Generic;
using System.Linq;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Strategy
{
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
			PlayingCard card = null;
			var hardMode = context.HardMode;
			var cards = context.FaceUpCards;


			var priority = Prioritize(context.FaceUpCards);
			foreach (var pile in priority)
			{
				if (hardMode)
				{
					if (pile.LastOrDefault()?.Rank != PlayingCardRank.Ace)
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

		private IEnumerable<PlayingCard[]> Prioritize(AcesUpFaceUpCards cards)
		{
			yield return cards.Pile1;
			yield return cards.Pile2;
			yield return cards.Pile3;
			yield return cards.Pile4;
		}

		private bool Peek(PlayingCard[] pile)
		{
			PlayingCard peek;
			PlayingCard card = null;
			if (pile.Length > 1 && (peek = pile.Last()) != null && peek.Rank == PlayingCardRank.Ace &&
			    (peek = pile.LastOrDefault()) != null)
			{
				card = peek;
			}

			return card != null;
		}

		private PlayingCard Pop(ref PlayingCard[] pile)
		{
			PlayingCard card;
			pile = pile.PopTopCard(out card);
			return card;
		}
	}
}