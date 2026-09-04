using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MonoDevelop.AnalysisCore
{
	internal sealed class RuleTreeRoot
	{
		private IRuleTreeNode[] children;

		private RuleTreeType treeType;

		public RuleTreeType TreeType => treeType;

		public RuleTreeRoot(IRuleTreeNode[] children, RuleTreeType treeType)
		{
			this.children = children;
			this.treeType = treeType;
		}

		public IEnumerable<Result> Analyze(object input, CancellationToken cancellationToken)
		{
			return children.SelectMany((IRuleTreeNode child) => child.Analyze(input, cancellationToken));
		}

		public string GetTreeStructure()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("[AnalysisTree (Input='{0}', Extension='{1}')\n", treeType.Input, treeType.FileExtension);
			PrintTreeStructure(stringBuilder, children, "  ");
			stringBuilder.Append("]\n");
			return stringBuilder.ToString();
		}

		private void PrintTreeStructure(StringBuilder builder, IRuleTreeNode[] children, string indent)
		{
			foreach (IRuleTreeNode ruleTreeNode in children)
			{
				builder.Append(indent);
				if (ruleTreeNode is RuleTreeLeaf ruleTreeLeaf)
				{
					builder.AppendFormat("[Leaf (Rule='{0}')]\n", ruleTreeLeaf.Rule.FuncName);
					continue;
				}
				RuleTreeBranch ruleTreeBranch = (RuleTreeBranch)ruleTreeNode;
				builder.AppendFormat("[Branch (Output='{0}',Rule='{1}')\n", ruleTreeBranch.Rule.Output, ruleTreeBranch.Rule.FuncName);
				PrintTreeStructure(builder, ruleTreeBranch.Children, indent + "  ");
				builder.Append(indent);
				builder.Append("]\n");
			}
		}
	}
}
