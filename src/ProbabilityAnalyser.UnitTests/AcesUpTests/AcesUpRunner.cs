using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProbabilityAnalyser.Core.Models;
using ProbabilityAnalyser.Core.Program.AcesUp;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests
{
	public class AcesUpRunner
	{
		public int LoopTimes { get; set; } = 1000;
		public bool UseParallelLoops { get; set; } = false;


		public AcesUpRunner()
		{
			if (System.Diagnostics.Debugger.IsAttached)
				LoopTimes = 1;
			//Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
		}


		private AcesUpRunConfig Init()
		{
			var config = new AcesUpRunConfig();
			return config;
		}

		private AcesUpRunManyConfig InitMany(int index, int times, bool parallel)
		{
			var config = new AcesUpRunManyConfig
			{
				LoopIndex = index,
				LoopTimes = times,
				UseParallelLoops = parallel,
			};
			return config;
		}



		public int Run(Action<AcesUpRunConfig> configure)
		{
			var cfg = Init();
			configure(cfg);

			var pts = InvokeProgram(cfg);
			return pts;
		}

		public int RunMany(Action<AcesUpRunConfig, int> configure, int? times = null, bool? parallel = null)
		{
			if (!times.HasValue || times < 1)
				times = LoopTimes;
			if (!parallel.HasValue)
				parallel = UseParallelLoops;


			var wins = 0;

			Action<int, ParallelLoopState> action;
			action = (index, parallelState) =>
			{
				var cfg = InitMany(index, times.Value, parallel.Value);
				configure(cfg, index);

				var pts = InvokeProgram(cfg);
				if (pts == 100)
					wins++;
			};

			InvokeLoop(parallel.Value, times.Value, action);

			return wins;
		}


		private ParallelLoopResult? InvokeLoop(bool parallel, int times, Action<int, ParallelLoopState> loop)
		{
			ParallelLoopResult? result = null;
			if (parallel)
			{
				result = Parallel.For(0, times, loop);
			}
			else
			{
				for (var i = 0; i < times; i++)
				{
					loop(i, null);
				}
			}
			return result;
		}



		private int InvokeProgram(AcesUpRunConfig config)
		{
			var deck = config.GetDeck();
			var cancellationToken = CancellationToken.None;
			var hardMode = false;



			var manyConfig = config as AcesUpRunManyConfig;
			TextWriter output = null;
			if (manyConfig == null || manyConfig.LoopTimes <= 1)
			{
				output = Console.Out;
			}

			Func<string, string> logFormatter;
			logFormatter = (s) =>
			{
				if (manyConfig != null)
				{
					if (manyConfig.LoopTimes > 1)
					{
						return $"[{manyConfig.LoopIndex:D5}] :: {s}";
					}
				}
				return s;
			};


			var ctx = new AcesUpRunContext(deck, cancellationToken, hardMode);

			var program = new AcesUp(output, logFormatter);
			var pts = program.Run(ctx);


			if (manyConfig == null || manyConfig.LoopTimes <= 1)
			{
				var msg = $"Points: {pts}";
				msg = logFormatter?.Invoke(msg) ?? msg;
				output?.WriteLine(msg);
			}

			return pts;
		}

	}



	public class AcesUpRunConfig
	{
		public AcesUpRunConfig()
		{
			GetDeck = PlayingCardDeck.Standard52CardDeck;
		}

		public Func<PlayingCardDeck> GetDeck;
		public Action<string> Console;
		public Action<string> Debug;
	}

	public class AcesUpRunManyConfig : AcesUpRunConfig
	{
		public int LoopIndex;
		public int LoopTimes;
		public bool UseParallelLoops;
	}
}
