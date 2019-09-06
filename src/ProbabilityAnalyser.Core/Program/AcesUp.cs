using System;
using System.Collections.Generic;
using System.Text;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program
{
	public class AcesUp
	{
		public object Run(PlayingCardDeck deck)
		{
			/*
				1. Deal four cards in a row face up.
				2. If there are two or more cards of the same suit, discard all but the highest-ranked card of that suit. Aces rank high.
				3. Repeat step 2 until there are no more pairs of cards with the same suit.
				4. Whenever there are any empty spaces, you may choose the top card of another pile to be put into the empty space. After you do this, go to Step 2.
				5. When there are no more cards to move or remove, deal out the next four cards from the deck face-up onto each pile.
				6. Repeat Step 2, using only the visible, or top, cards on each of the four piles.
				7. When the last four cards have been dealt out and any moves made, the game is over. The fewer cards left in the tableau, the better. To win is to have only the four aces left.
				
				When the game ends, the number of discarded cards is your score. The maximum score (and thus the score necessary to win) is 48, which means all cards have been discarded except for the four aces, thus the name of the game.
			*/


			object result = null;

			int loops = 0;
			while (deck.IsEmpty)
			{
				// 1. Deal four cards in a row face up.
				var drawnCards = deck.DrawMany(4);



				loops++;
			}

			result = loops;


			return result;
		}
	}
}
