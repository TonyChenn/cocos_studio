namespace MonoDevelop.CodeIssues
{
	[GroupingDescription("CodeIssue")]
	public class ProviderGroupingProvider : AbstractGroupingProvider<string>
	{
		protected override string GetGroupingKey(IssueSummary issue)
		{
			return issue.ProviderTitle;
		}

		protected override string GetGroupName(IssueSummary issue)
		{
			return issue.ProviderTitle;
		}
	}
}
