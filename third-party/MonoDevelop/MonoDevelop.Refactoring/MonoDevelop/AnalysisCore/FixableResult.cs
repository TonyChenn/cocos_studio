using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;

namespace MonoDevelop.AnalysisCore
{
	public class FixableResult : Result
	{
		public IAnalysisFix[] Fixes { get; protected set; }

		public FixableResult(DomRegion region, string message, Severity level, IssueMarker mark, params IAnalysisFix[] fixes)
			: base(region, message, level, mark)
		{
			Fixes = fixes;
		}
	}
}
