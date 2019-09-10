using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ProbabilityAnalyser.UnitTests
{
	[TestFixture]
	public class SpecificationBase
	{
		[OneTimeSetUp]
		protected virtual void Init()
		{

		}

		[SetUp]
		protected virtual void Setup()
		{

		}

		[TearDown]
		protected virtual void Cleanup()
		{

		}

	}
}
