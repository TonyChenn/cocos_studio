using System;

namespace MonoDevelop.CodeIssues
{
	public class IssueTreeNodeEventArgs : EventArgs
	{
		public IIssueTreeNode Parent { get; private set; }

		public IIssueTreeNode Child { get; private set; }

		public IssueTreeNodeEventArgs(IIssueTreeNode parent, IIssueTreeNode child)
		{
			Parent = parent;
			Child = child;
		}
	}
}
