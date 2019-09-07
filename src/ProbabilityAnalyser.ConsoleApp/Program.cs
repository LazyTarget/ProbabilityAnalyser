using System;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program;

namespace ProbabilityAnalyser.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("ProbabilityAnalyser!");
			try
			{
				Run();
			}
			catch (Exception ex)
			{
				var err = ex.ToString();
				Console.WriteLine("Exception: " + err);
			}
			finally
			{
				Console.WriteLine("Press [Enter] to exit...");
				Console.ReadLine();
			}
		}

		private static void Run()
		{
			var deck = PlayingCardDeck.Standard52CardDeck();

			var program = new AcesUp();
			var result = program.Run(deck);

			Console.WriteLine("AcesUp :: Result = {0}", result);
		}

	}
}
