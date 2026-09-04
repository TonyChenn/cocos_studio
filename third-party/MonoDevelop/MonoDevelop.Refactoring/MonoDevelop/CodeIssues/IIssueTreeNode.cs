using System;
using System.Collections.Generic;

namespace MonoDevelop.CodeIssues
{
	public interface IIssueTreeNode
	{
		string Text { get; }

		ICollection<IIssueTreeNode> Children { get; }

		bool HasVisibleChildren { get; }

		bool Visible { get; set; }

		ICollection<IIssueTreeNode> AllChildren { get; }

		event EventHandler<IssueGroupEventArgs> ChildrenInvalidated;

		event EventHandler<IssueTreeNodeEventArgs> ChildAdded;

		event EventHandler<IssueGroupEventArgs> TextChanged;

		event EventHandler<IssueGroupEventArgs> VisibleChanged;
	}
}
