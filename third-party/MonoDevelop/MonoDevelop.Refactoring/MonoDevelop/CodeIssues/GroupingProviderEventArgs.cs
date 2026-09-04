using System;

namespace MonoDevelop.CodeIssues
{
	public class GroupingProviderEventArgs : EventArgs
	{
		public IGroupingProvider GroupingProvider { get; private set; }

		public IGroupingProvider OldNext { get; private set; }

		public GroupingProviderEventArgs(IGroupingProvider provider, IGroupingProvider oldNext)
		{
			GroupingProvider = provider;
			OldNext = oldNext;
		}
	}
}
