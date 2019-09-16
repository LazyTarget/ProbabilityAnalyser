using System;
using ProbabilityAnalyser.Core.Intefaces;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Helpers
{
	public class PlayingCardBuilder : IPlayingCardBuilder, ISuitedPlayingCardBuilder
	{
		private PlayingCardRank _rank;
		private PlayingCardSuit _suit;

		public PlayingCardBuilder()
		{
			
		}

		protected void Clear()
		{
			_rank = PlayingCardRank.None;
			_suit = PlayingCardSuit.None;
		}


		public IPlayingCardBuilder New()
		{
			Clear();
			return this;
		}

		public ISuitedPlayingCardBuilder Clubs()
		{
			_suit = PlayingCardSuit.Clubs;
			return this;
		}

		public ISuitedPlayingCardBuilder Spades()
		{
			_suit = PlayingCardSuit.Spades;
			return this;
		}

		public ISuitedPlayingCardBuilder Diamonds()
		{
			_suit = PlayingCardSuit.Diamonds;
			return this;
		}

		public ISuitedPlayingCardBuilder Heart()
		{
			_suit = PlayingCardSuit.Hearts;
			return this;
		}

		public PlayingCard Joker()
		{
			var card = PlayingCard.Joker;
			return card;
		}


		public PlayingCard Rank(PlayingCardRank rank)
		{
			if (_suit == PlayingCardSuit.None)
				throw new ArgumentException("Suit has not been set");

			_rank = rank;
			var card = new PlayingCard(_suit, rank);
			return card;
		}
	}
}
