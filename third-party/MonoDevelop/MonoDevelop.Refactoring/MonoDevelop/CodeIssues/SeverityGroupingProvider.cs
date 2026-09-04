using ICSharpCode.NRefactory.Refactoring;

namespace MonoDevelop.CodeIssues
{
	[GroupingDescription("Severity")]
	public class SeverityGroupingProvider : AbstractGroupingProvider<Severity>
	{
		protected override Severity GetGroupingKey(IssueSummary issue)
		{
			return issue.Severity;
		}

		protected override string GetGroupName(IssueSummary issue)
		{
			return issue.Severity.ToString();
		}
	}
}
