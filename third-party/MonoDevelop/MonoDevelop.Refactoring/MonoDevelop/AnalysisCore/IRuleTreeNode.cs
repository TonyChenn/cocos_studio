using System.Collections.Generic;
using System.Threading;

namespace MonoDevelop.AnalysisCore
{
	internal interface IRuleTreeNode
	{
		IEnumerable<Result> Analyze(object input, CancellationToken cancellationToken);
	}
}
