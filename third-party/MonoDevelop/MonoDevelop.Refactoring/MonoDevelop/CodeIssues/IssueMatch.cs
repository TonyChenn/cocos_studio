using MonoDevelop.CodeActions;

namespace MonoDevelop.CodeIssues
{
	public class IssueMatch
	{
		public ActionSummary Summary { get; set; }

		public CodeAction Action { get; set; }
	}
}
