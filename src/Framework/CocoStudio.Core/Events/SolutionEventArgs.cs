using System;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	public class SolutionEventArgs : WorkspaceItemEventArgs
	{
		public SolutionEventArgs(Solution sol) : base(sol)
		{
		}

		public Solution Solution
		{
			get
			{
				return (Solution)base.Item;
			}
		}
	}
}
