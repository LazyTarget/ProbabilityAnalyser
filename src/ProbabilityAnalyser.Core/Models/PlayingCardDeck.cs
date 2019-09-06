using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProbabilityAnalyser.Core.Models
{
	public class PlayingCardDeck
	{
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


		public PlayingCardDeck(IEnumerable<PlayingCard> cards)
		{
			Cards = cards.ToArray();
		}

		public PlayingCard[] Cards { get; }
	}
}
