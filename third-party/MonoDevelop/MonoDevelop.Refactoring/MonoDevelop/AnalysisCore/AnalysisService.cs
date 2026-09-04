using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonoDevelop.AnalysisCore
{
	public static class AnalysisService
	{
		public static IEnumerable<Result> Analyze<T>(T input, RuleTreeType treeType, CancellationToken cancellationToken = default(CancellationToken))
		{
			RuleTreeRoot analysisTree = AnalysisExtensions.GetAnalysisTree(treeType);
			if (analysisTree == null)
			{
				return RuleTreeLeaf.Empty;
			}
			return analysisTree.Analyze(input, cancellationToken).ToList();
		}

		public static Task<IEnumerable<Result>> QueueAnalysis<T>(T input, RuleTreeType treeType, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Factory.StartNew(() => Analyze(input, treeType, cancellationToken), cancellationToken);
		}
	}
}
