using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProbabilityAnalyser.Core.Models
{
	public class PlayingCardDeck
	{
		#region Static

		private static readonly Random _random = new Random();
		public static readonly PlayingCardDeck Standard52CardDeck;


		static PlayingCardDeck()
		{
			var standard = new List<PlayingCard>();

			var suits = Enum.GetValues(typeof(PlayingCardSuit)).OfType<PlayingCardSuit>().ToArray();
			var ranks = Enum.GetValues(typeof(PlayingCardRank)).OfType<PlayingCardRank>().ToArray();
			for (var s = 0; s < suits.Length; s++)
			{
				var suit = suits[s];
				if (suit == PlayingCardSuit.None)
					continue;
				if (suit == PlayingCardSuit.Joker)
					continue;

				for (var r = 0; r < ranks.Length; r++)
				{
					var rank = ranks[r];
					if (rank == PlayingCardRank.None)
						continue;
					if (rank == PlayingCardRank.Joker)
						continue;

					var card = new PlayingCard(suit, rank);
					standard.Add(card);
				}
			}



			Standard52CardDeck = new PlayingCardDeck(
				standard.ToArray()
			);
		}

		#endregion


		public PlayingCardDeck(IEnumerable<PlayingCard> cards)
		{
			Cards = cards.ToArray();
		}

		public PlayingCard[] Cards { get; private set; }

		public bool IsEmpty => Cards.Length <= 0;


		public void Shuffle()
		{
			var shuffled = Cards.OrderBy(c => _random.Next()).ToArray();
			Cards = shuffled;
		}

		public PlayingCard Draw()
		{
			PlayingCard card;
			if (Cards.Length < 1)
			{
				card = null;
			}
			else
			{
				var index = Cards.Length - 1;
				card = Cards[index];
				var cards = Cards.Take(index).ToArray();
				Cards = cards;
			}
			return card;
		}

		public PlayingCard[] DrawMany(int count)
		{
			var list = new List<PlayingCard>();
			for (var i = 0; i < count; i++)
			{
				var card = Draw();
				if (card == null)
					break;
				list.Add(card);
			}
			return list.ToArray();
		}

	}
}
