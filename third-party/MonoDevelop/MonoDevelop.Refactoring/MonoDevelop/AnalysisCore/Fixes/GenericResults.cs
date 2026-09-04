using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;

namespace MonoDevelop.AnalysisCore.Fixes
{
	public class GenericResults : FixableResult
	{
		public GenericResults(DomRegion region, string message, Severity level, IssueMarker mark, params GenericFix[] fixes)
			: base(region, message, level, mark)
		{
			base.Fixes = fixes;
		}
	}
}
