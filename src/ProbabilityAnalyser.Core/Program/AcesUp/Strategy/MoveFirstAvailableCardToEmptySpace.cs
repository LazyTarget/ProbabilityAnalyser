using System;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Strategy
{
	public class MoveFirstAvailableCardToEmptySpace : ICardMovingStrategy
	{
		private readonly ICardMovingStrategy _fallback;

		public MoveFirstAvailableCardToEmptySpace(ICardMovingStrategy fallback = null)
		{
			_fallback = fallback;
		}

		public virtual bool MoveCard(AcesUpRunContext context)
		{
			bool moved;
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

				moved = false;
				if (_fallback != null)
					moved = _fallback.MoveCard(context);
				return moved;
			}

			// Move card...
			moved = context.FaceUpCards.AppendOneToEmptyPile(card);
			if (!moved)
			{
				throw new Exception($"Card was popped from a Pile but could not be Appended to an empty pile!");
			}

			return moved;
		}
	}
}