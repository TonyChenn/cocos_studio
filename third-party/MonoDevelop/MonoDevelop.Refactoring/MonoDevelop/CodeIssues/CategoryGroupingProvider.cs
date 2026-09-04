namespace MonoDevelop.CodeIssues
{
	[GroupingDescription("Category")]
	public class CategoryGroupingProvider : AbstractGroupingProvider<string>
	{
		protected override string GetGroupingKey(IssueSummary issue)
		{
			return issue.ProviderCategory;
		}

		protected override string GetGroupName(IssueSummary issue)
		{
			return issue.ProviderCategory;
		}
	}
}
