using System;

namespace MonoDevelop.CodeIssues
{
	public class IssueGroupEventArgs : EventArgs
	{
		public IIssueTreeNode Node { get; private set; }

		public IssueGroupEventArgs(IIssueTreeNode node)
		{
			Node = node;
		}
	}
}
