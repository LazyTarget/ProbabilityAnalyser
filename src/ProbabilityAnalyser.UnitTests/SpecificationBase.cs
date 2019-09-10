using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProbabilityAnalyser.UnitTests
{
	[TestClass]
	public class SpecificationBase
	{
		[ClassInitialize]
		protected virtual void Init()
		{

		}

		[TestInitialize]
		protected virtual void Setup()
		{

		}

		[TestCleanup]
		protected virtual void Cleanup()
		{

		}

	}
}
