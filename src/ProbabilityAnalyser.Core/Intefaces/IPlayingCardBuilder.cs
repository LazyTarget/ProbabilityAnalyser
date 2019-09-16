using System;
using System.Collections.Generic;
using System.Text;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Intefaces
{
	public interface IPlayingCardBuilder
	{
		ISuitedPlayingCardBuilder Clubs();
		ISuitedPlayingCardBuilder Spades();
		ISuitedPlayingCardBuilder Diamonds();
		ISuitedPlayingCardBuilder Hearts();
		PlayingCard Joker();
	}

	public interface ISuitedPlayingCardBuilder
	{
		PlayingCard Rank(PlayingCardRank rank);
	}
}
