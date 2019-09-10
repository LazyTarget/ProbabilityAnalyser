using System;
using System.Linq;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp.Strategy
{
	public class MoveCardBasedOnDirectlyUnderTopCard : CardMovingStrategyBase
	{
		private readonly ICardMovingStrategy _fallback;

		public MoveCardBasedOnDirectlyUnderTopCard(ICardMovingStrategy fallback = null)
		{
			_fallback = fallback;
		}

		protected override bool Peek(PlayingCard[] pile)
		{
			PlayingCard peek;
			PlayingCard card = null;
			if (pile.Length > 1 && (peek = pile.Reverse().ElementAtOrDefault(1)) != null)
			{
				

				card = peek;
			}

			return card != null;
		}

		public virtual bool MoveCard(AcesUpRunContext context)
		{
			// todo: Implement "AI" which remembers the card under the Top card(s), for better 'moving-strategy'

			var top = context.FaceUpCards.Top().ToArray();
			if (top.Length >= 4)
				return false;       // no change

			var suitsOnTop = top.Select(c => c.Suit).Distinct().ToArray();

			bool moved;
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