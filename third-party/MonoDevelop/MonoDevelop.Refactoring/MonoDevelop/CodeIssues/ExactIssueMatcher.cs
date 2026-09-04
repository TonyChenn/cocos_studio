using System.Collections.Generic;
using System.Linq;
using Mono.TextEditor;
using MonoDevelop.CodeActions;

namespace MonoDevelop.CodeIssues
{
	public class ExactIssueMatcher : IActionMatcher
	{
		public IEnumerable<IssueMatch> Match(IList<ActionSummary> summaries, IList<CodeAction> realActions)
		{
			ILookup<DocumentRegion, ActionSummary> summaryLookup = summaries.ToLookup((ActionSummary summary) => summary.Region);
			foreach (CodeAction action in realActions)
			{
				if (summaryLookup.Contains(action.DocumentRegion))
				{
					yield return new IssueMatch
					{
						Action = action,
						Summary = summaryLookup[action.DocumentRegion].First()
					};
				}
			}
		}
	}
}
