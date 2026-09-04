using System.Collections.Generic;
using System.Threading;
using MonoDevelop.AnalysisCore.Extensions;

namespace MonoDevelop.AnalysisCore
{
	internal sealed class RuleTreeLeaf : IRuleTreeNode
	{
		internal const string TYPE = "Results";

		private AnalysisRuleAddinNode rule;

		public static Result[] Empty = new Result[0];

		public AnalysisRuleAddinNode Rule => rule;

		public RuleTreeLeaf(AnalysisRuleAddinNode rule)
		{
			this.rule = rule;
		}

		public IEnumerable<Result> Analyze(object input, CancellationToken cancellationToken)
		{
			IEnumerable<Result> enumerable = (IEnumerable<Result>)rule.Analyze(input, cancellationToken);
			if (enumerable == null)
			{
				return Empty;
			}
			foreach (Result item in enumerable)
			{
				item.Source = rule;
			}
			return enumerable;
		}
	}
}
