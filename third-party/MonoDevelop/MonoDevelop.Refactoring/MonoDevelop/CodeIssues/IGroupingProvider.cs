using System;

namespace MonoDevelop.CodeIssues
{
	public interface IGroupingProvider
	{
		IGroupingProvider Next { get; set; }

		bool SupportsNext { get; }

		event EventHandler<GroupingProviderEventArgs> NextChanged;

		IssueGroup GetIssueGroup(IssueGroup parentGroup, IssueSummary issue);

		void Reset();
	}
}
