using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDevelop.CodeIssues
{
	public class IssueGroup : IIssueTreeNode, IIssueSummarySink
	{
		private readonly object _lock = new object();

		private bool processingEnabled;

		private readonly ISet<IssueGroup> groups = new HashSet<IssueGroup>();

		private readonly IList<IIssueTreeNode> children = new List<IIssueTreeNode>();

		private readonly ISet<IssueSummary> allIssues = new HashSet<IssueSummary>();

		private IGroupingProvider groupingProvider;

		public IGroupingProvider GroupingProvider
		{
			get
			{
				return groupingProvider;
			}
			set
			{
				lock (_lock)
				{
					processingEnabled = false;
					groupingProvider = value;
					groups.Clear();
					children.Clear();
				}
				OnChildrenInvalidated(new IssueGroupEventArgs(this));
			}
		}

		string IIssueTreeNode.Text
		{
			get
			{
				lock (_lock)
				{
					return $"{Description} ({allIssues.Count((IssueSummary issue) => ((IIssueTreeNode)issue).Visible)})";
				}
			}
		}

		ICollection<IIssueTreeNode> IIssueTreeNode.Children
		{
			get
			{
				EnableProcessing();
				lock (_lock)
				{
					return new List<IIssueTreeNode>(children);
				}
			}
		}

		bool IIssueTreeNode.HasVisibleChildren
		{
			get
			{
				lock (_lock)
				{
					return allIssues.Any((IssueSummary issue) => ((IIssueTreeNode)issue).Visible);
				}
			}
		}

		bool IIssueTreeNode.Visible
		{
			get
			{
				return ((IIssueTreeNode)this).HasVisibleChildren;
			}
			set
			{
				throw new InvalidOperationException("Not supported");
			}
		}

		ICollection<IIssueTreeNode> IIssueTreeNode.AllChildren
		{
			get
			{
				lock (_lock)
				{
					return new List<IIssueTreeNode>(allIssues);
				}
			}
		}

		public string Description { get; private set; }

		public int IssueCount { get; private set; }

		private event EventHandler<IssueGroupEventArgs> childrenInvalidated;

		event EventHandler<IssueGroupEventArgs> IIssueTreeNode.ChildrenInvalidated
		{
			add
			{
				childrenInvalidated += value;
			}
			remove
			{
				childrenInvalidated -= value;
			}
		}

		private event EventHandler<IssueTreeNodeEventArgs> childAdded;

		event EventHandler<IssueTreeNodeEventArgs> IIssueTreeNode.ChildAdded
		{
			add
			{
				childAdded += value;
			}
			remove
			{
				childAdded -= value;
			}
		}

		private event EventHandler<IssueGroupEventArgs> textChanged;

		event EventHandler<IssueGroupEventArgs> IIssueTreeNode.TextChanged
		{
			add
			{
				textChanged += value;
			}
			remove
			{
				textChanged -= value;
			}
		}

		private event EventHandler<IssueGroupEventArgs> visibleChanged;

		event EventHandler<IssueGroupEventArgs> IIssueTreeNode.VisibleChanged
		{
			add
			{
				visibleChanged += value;
			}
			remove
			{
				visibleChanged -= value;
			}
		}

		public IssueGroup(IGroupingProvider nextProvider, string description)
		{
			groupingProvider = nextProvider;
			Description = description;
			processingEnabled = false;
		}

		protected virtual void OnChildrenInvalidated(IssueGroupEventArgs eventArgs)
		{
			childrenInvalidated?.Invoke(this, eventArgs);
		}

		protected virtual void OnChildAdded(IssueTreeNodeEventArgs eventArgs)
		{
			childAdded?.Invoke(this, eventArgs);
		}

		protected virtual void OnTextChanged(IssueGroupEventArgs eventArgs)
		{
			textChanged?.Invoke(this, eventArgs);
		}

		protected virtual void OnVisibleChanged(IssueGroupEventArgs eventArgs)
		{
			visibleChanged?.Invoke(this, eventArgs);
		}

		public void ClearStatistics()
		{
			lock (_lock)
			{
				groups.Clear();
				children.Clear();
				allIssues.Clear();
				groupingProvider.Reset();
				IssueCount = 0;
				processingEnabled = false;
			}
		}

		public void EnableProcessing()
		{
			lock (_lock)
			{
				if (processingEnabled)
				{
					return;
				}
				processingEnabled = true;
				foreach (IssueSummary allIssue in allIssues)
				{
					ProcessIssue(allIssue, out var group);
					group?.AddIssue(allIssue);
				}
			}
		}

		public void AddIssue(IssueSummary issue)
		{
			IssueGroup group = null;
			bool flag = false;
			bool flag2 = false;
			lock (_lock)
			{
				if (!allIssues.Contains(issue))
				{
					IssueCount++;
					allIssues.Add(issue);
					((IIssueTreeNode)issue).VisibleChanged += HandleVisibleChanged;
					flag2 = true;
				}
				if (processingEnabled)
				{
					flag = ProcessIssue(issue, out group);
				}
			}
			if (flag2)
			{
				OnTextChanged(new IssueGroupEventArgs(this));
			}
			if (processingEnabled)
			{
				if (flag)
				{
					OnChildAdded(new IssueTreeNodeEventArgs(this, group));
				}
				else if (group == null)
				{
					OnChildAdded(new IssueTreeNodeEventArgs(this, issue));
				}
				group?.AddIssue(issue);
			}
		}

		private void HandleVisibleChanged(object sender, IssueGroupEventArgs e)
		{
			lock (_lock)
			{
				bool flag = children.Any((IIssueTreeNode child) => child.Visible);
				if ((e.Node.Visible && flag) || (!e.Node.Visible && !flag))
				{
					OnVisibleChanged(new IssueGroupEventArgs(this));
				}
				OnTextChanged(new IssueGroupEventArgs(this));
			}
		}

		private bool ProcessIssue(IssueSummary issue, out IssueGroup group)
		{
			bool result = false;
			group = null;
			if (groupingProvider != null)
			{
				group = groupingProvider.GetIssueGroup(this, issue);
			}
			if (group == null)
			{
				children.Add(issue);
			}
			else if (!groups.Contains(group))
			{
				result = true;
				groups.Add(group);
				children.Add(group);
			}
			return result;
		}

		public override string ToString()
		{
			return string.Format("[IssueGroup: Description={1}, IssueCount={2}, GroupingProvider={0}]", GroupingProvider, Description, IssueCount);
		}
	}
}
