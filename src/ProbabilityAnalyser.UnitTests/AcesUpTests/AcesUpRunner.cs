using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProbabilityAnalyser.Core.Extensions;
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


		protected virtual AcesUpRunConfig Init()
		{
			var config = new AcesUpRunConfig();
			return config;
		}

		protected virtual AcesUpRunManyConfig InitMany(int index, int times, bool parallel)
		{
			var config = new AcesUpRunManyConfig
			{
				LoopIndex = index,
				LoopTimes = times,
				UseParallelLoops = parallel,
			};
			return config;
		}



		public virtual int Run(Action<AcesUpRunConfig> configure)
		{
			var cfg = Init();
			configure(cfg);

			var pts = InvokeProgram(cfg);
			return pts;
		}

		public virtual int RunMany(Action<AcesUpRunConfig, int> configure, int? times = null, bool? parallel = null)
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


		protected virtual ParallelLoopResult? InvokeLoop(bool parallel, int times, Action<int, ParallelLoopState> loop)
		{
			var result = CommonExtensions.InvokeLoop(parallel, times, loop);
			return result;
		}



		protected virtual int InvokeProgram(AcesUpRunConfig config)
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
			if (config.ConfigureArguments != null)
			{
				config.ConfigureArguments(ctx);
			}

			IAcesUp program;
			if (config.UseObsolete)
				program = new AcesUpOld(output, logFormatter);
			else
				program = new AcesUp(output, logFormatter);

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
			var deck = PlayingCardDeck.Standard52CardDeck(shuffle: true);
			GetDeck = () => deck;
		}

		public Func<PlayingCardDeck> GetDeck;
		public Action<AcesUpRunContext> ConfigureArguments;
		public Action<string> Console;
		public Action<string> Debug;

		public bool UseObsolete = true;
	}

	public class AcesUpRunManyConfig : AcesUpRunConfig
	{
		public int LoopIndex;
		public int LoopTimes;
		public bool UseParallelLoops;
	}
}
