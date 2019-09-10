using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProbabilityAnalyser.Core.Models;

namespace ProbabilityAnalyser.Core.Program.AcesUp
{
	public class AcesUpPile : IEnumerable<PlayingCard>
	{
		public AcesUpPile(AcesUpRunContext context)
		{
			Pile = new PlayingCard[0];
			Context = context;
		}

		public PlayingCard[] Pile;
		public readonly AcesUpRunContext Context;


		public int Length => Pile?.Length ?? 0;

		public IEnumerator<PlayingCard> GetEnumerator()
		{
			var array = Pile ?? new PlayingCard[0];
			var enumerator = array.AsEnumerable().GetEnumerator();
			return enumerator;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
