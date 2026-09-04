using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MonoDevelop.AnalysisCore.Extensions;

namespace MonoDevelop.AnalysisCore
{
	internal sealed class RuleTreeBranch : IRuleTreeNode
	{
		private AnalysisRuleAddinNode rule;

		private IRuleTreeNode[] children;

		public AnalysisRuleAddinNode Rule => rule;

		public IRuleTreeNode[] Children => children;

		public RuleTreeBranch(IRuleTreeNode[] children, AnalysisRuleAddinNode rule)
		{
			this.rule = rule;
			this.children = children;
		}

		public IEnumerable<Result> Analyze(object input, CancellationToken cancellationToken = default(CancellationToken))
		{
			object intermediate = rule.Analyze(input, cancellationToken);
			if (intermediate == null)
			{
				return RuleTreeLeaf.Empty;
			}
			return children.SelectMany((IRuleTreeNode child) => child.Analyze(intermediate, cancellationToken));
		}
	}
}
