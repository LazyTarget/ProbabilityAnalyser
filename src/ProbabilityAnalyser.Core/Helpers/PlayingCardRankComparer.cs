using System.Collections.Generic;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Extensions
{
	public class PlayingCardRankComparer : IComparer<PlayingCardRank>
	{
		public bool AceRankHigh { get; set; } = true;

		public int Compare(PlayingCardRank x, PlayingCardRank y)
		{
			var a = (int)x;
			var b = (int)y;

			if (x == PlayingCardRank.Ace)
				a = AceRankHigh ? 14 : 1;
			if (y == PlayingCardRank.Ace)
				b = AceRankHigh ? 14 : 1;

			var c = a.CompareTo(b);
			return c;
		}
	}
}