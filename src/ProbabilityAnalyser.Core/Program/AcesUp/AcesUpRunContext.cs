﻿using System.Threading;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp.Strategy;

namespace ProbabilityAnalyser.Core.Program.AcesUp
{
	public class AcesUpRunContext
	{
		public AcesUpRunContext(PlayingCardDeck deck, CancellationToken cancellationToken, bool hardMode = false)
		{
			Deck = deck;
			Token = cancellationToken;
			HardMode = hardMode;
			MovingStrategy =
				new MoveCardBasedOnDirectlyUnderTopCard(
					new MoveFirstAvailableCardToEmptySpace()
				);
			FaceUpCards = new AcesUpFaceUpCards(this);
		}

		public PlayingCardDeck Deck { get; }
		public AcesUpFaceUpCards FaceUpCards { get; }
		public CancellationToken Token { get; }
		public bool HardMode { get; set; }

		public ICardMovingStrategy MovingStrategy;
	}
}