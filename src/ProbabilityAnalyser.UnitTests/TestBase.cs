using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace ProbabilityAnalyser.UnitTests
{
	[TestFixture]
	public abstract class TestBase
	{
		private Stopwatch _stopwatch;

		[OneTimeSetUp]
		protected virtual void OneTimeSetup()
		{

		}

		[SetUp]
		protected virtual void Setup()
		{
			if (_stopwatch == null)
			{
				_stopwatch = new Stopwatch();
				_stopwatch.Start();
			}
		}

		[TearDown]
		protected virtual void Cleanup()
		{
			var context = TestContext.CurrentContext;
			if (_stopwatch != null)
			{
				_stopwatch.Stop();
				//Trace.WriteLine($"Test \"{context.Test.FullName}\" ended with duration {_stopwatch.Elapsed}");
				Trace.WriteLine($"Test ended with duration {_stopwatch.Elapsed}");
				_stopwatch = null;
			}
		}

	}
}
