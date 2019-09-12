using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using ProbabilityAnalyser.Core.Program.AcesUp;

namespace ProbabilityAnalyser.UnitTests.AcesUpTests.Helpers
{
	public class AcesUpStrategyBuilder
	{
		private StringWriter _sw;
		private StringBuilder _sb;
		private ICardMovingStrategy _strategy;
		private IPilePrioritizer _prioritizer;

		public AcesUpStrategyBuilder()
		{
			_sb = new StringBuilder();
			_sw = new StringWriter(_sb);
		}

		protected void Clear()
		{
			_sb = new StringBuilder();
			_sw = new StringWriter(_sb);
			_strategy = null;
			_prioritizer = null;
		}

		public AcesUpStrategyBuilder AppendStrategy<TStrategy>()
			where TStrategy : ICardMovingStrategy
		{
			return AppendStrategy(typeof(TStrategy));
		}

		public AcesUpStrategyBuilder AppendStrategy(params Type[] types)
		{
			if (types != null)
			{
				foreach (var t in types)
				{
					if (t == null)
						continue;

					if (_strategy != null)
					{
						_sb.Append(" > ");
					}


					object[] args = new[] { _strategy };
					var obj = Activator.CreateInstance(t, args);
					_strategy = (ICardMovingStrategy)obj;

					var displayName = GetDisplayNameForInstance(_strategy);
					_sb.Append($"{displayName}");
				}
			}
			return this;
		}

		public AcesUpStrategyBuilder Prioritizer<T>()
			where T : IPilePrioritizer, new()
		{
			_prioritizer = new T();
			return this;
		}

		public AcesUpStrategyBuilder Prioritizer(IPilePrioritizer prioritizer)
		{
			_prioritizer = prioritizer;
			return this;
		}

		public AcesUpStrategyBuilder New()
		{
			Clear();
			return this;
		}

		public AcesUpTests.AcesUpArgCombination Peek()
		{
			var result = new AcesUpTests.AcesUpArgCombination();
			result.Prioritizer = _prioritizer;
			result.MovingStrategy = _strategy;
			result.FriendlyName = GetGeneratedDisplayName();
			return result;
		}

		public AcesUpTests.AcesUpArgCombination Build()
		{
			var result = Peek();
			Clear();
			return result;
		}


		private string GetDisplayNameForInstance(object obj)
		{
			string str;
			var type = obj?.GetType();
			var displayNameAttr = type?.GetCustomAttribute<DisplayNameAttribute>();
			if (displayNameAttr?.DisplayName != null)
			{
				str = displayNameAttr.DisplayName;
			}
			else if (type?.Name != null)
			{
				str = type.Name;
			}
			else
			{
				str = obj?.ToString();
			}
			return str;
		}


		private string GetGeneratedDisplayName()
		{
			var strategies = _sb.ToString();

			string result;
			var prioritizer = GetDisplayNameForInstance(_prioritizer);
			if (prioritizer != null)
				result = $"{strategies} [{prioritizer}]";
			else
				result = $"{strategies}";
			return result;
		}
	}
}
