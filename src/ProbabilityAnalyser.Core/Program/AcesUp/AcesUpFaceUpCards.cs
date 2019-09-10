using System;
using System.Collections.Generic;
using System.Linq;
using ProbabilityAnalyser.Core.Extensions;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp
{
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


		public bool AppendOneToEmptyPile(PlayingCard card)
		{
			bool appended = true;
			if (Pile1.Length <= 0)
				AppendCardToPile(card, 1);
			else if (!Pile2.Any())
				AppendCardToPile(card, 2);
			else if (!Pile3.Any())
				AppendCardToPile(card, 3);
			else if (!Pile4.Any())
				AppendCardToPile(card, 4);
			else
				appended = false;
			return appended;
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
}