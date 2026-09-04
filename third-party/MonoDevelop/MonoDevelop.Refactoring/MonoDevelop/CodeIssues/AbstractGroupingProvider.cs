using System;
using System.Collections.Generic;

namespace MonoDevelop.CodeIssues
{
	public abstract class AbstractGroupingProvider<T> : IGroupingProvider
	{
		private readonly Dictionary<Tuple<IssueGroup, T>, IssueGroup> groups = new Dictionary<Tuple<IssueGroup, T>, IssueGroup>();

		private IGroupingProvider next;

		public IGroupingProvider Next
		{
			get
			{
				return next;
			}
			set
			{
				GroupingProviderEventArgs eventArgs = new GroupingProviderEventArgs(this, next);
				next = value;
				foreach (IssueGroup value2 in groups.Values)
				{
					value2.GroupingProvider = value;
				}
				OnNextChanged(eventArgs);
			}
		}

		public bool SupportsNext => true;

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

		protected AbstractGroupingProvider()
		{
			Next = NullGroupingProvider.Instance;
		}

		protected abstract T GetGroupingKey(IssueSummary issue);

		protected abstract string GetGroupName(IssueSummary issue);

		public IssueGroup GetIssueGroup(IssueGroup parentGroup, IssueSummary issue)
		{
			T groupingKey = GetGroupingKey(issue);
			if (groupingKey == null)
			{
				return null;
			}
			Tuple<IssueGroup, T> key = Tuple.Create(parentGroup, groupingKey);
			if (!groups.TryGetValue(key, out var value))
			{
				value = new IssueGroup(Next, GetGroupName(issue));
				groups.Add(key, value);
			}
			return value;
		}

		public void Reset()
		{
			groups.Clear();
			Next.Reset();
		}

		protected virtual void OnNextChanged(GroupingProviderEventArgs eventArgs)
		{
			nextChanged?.Invoke(this, eventArgs);
		}
	}
}
