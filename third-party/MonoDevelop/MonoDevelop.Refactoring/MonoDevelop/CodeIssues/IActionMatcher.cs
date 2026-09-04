using System.Collections.Generic;
using MonoDevelop.CodeActions;

namespace MonoDevelop.CodeIssues
{
	public interface IActionMatcher
	{
		IEnumerable<IssueMatch> Match(IList<ActionSummary> summaries, IList<CodeAction> realIssues);
	}
}
