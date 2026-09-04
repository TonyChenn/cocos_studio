using System;

namespace MonoDevelop.CodeIssues
{
	public class NullGroupingProvider : IGroupingProvider
	{
		private static readonly Lazy<NullGroupingProvider> instance = new Lazy<NullGroupingProvider>();

		public static IGroupingProvider Instance => instance.Value;

		public IGroupingProvider Next
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public bool SupportsNext => false;

		private event EventHandler<GroupingProviderEventArgs> nextChanged;

		event EventHandler<GroupingProviderEventArgs> IGroupingProvider.NextChanged
		{
			add
			{
				nextChanged += value;
			}
			remove
			{
				nextChanged -= value;
			}
		}

		public IssueGroup GetIssueGroup(IssueGroup parent, IssueSummary issue)
		{
			return null;
		}

		public void Reset()
		{
		}
	}
}
