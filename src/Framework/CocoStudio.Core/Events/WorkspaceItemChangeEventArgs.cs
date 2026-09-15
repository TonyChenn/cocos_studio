using System;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	public class WorkspaceItemChangeEventArgs : WorkspaceItemEventArgs
	{
		public WorkspaceItemChangeEventArgs(WorkspaceItem item, bool reloading) : base(item)
		{
			this.reloading = reloading;
		}

		public bool Reloading
		{
			get
			{
				return this.reloading;
			}
		}

		private bool reloading;
	}
}
