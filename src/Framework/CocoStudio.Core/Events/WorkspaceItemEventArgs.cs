using System;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	public class WorkspaceItemEventArgs : EventArgs
	{
		public WorkspaceItem Item
		{
			get
			{
				return this.item;
			}
		}

		public WorkspaceItemEventArgs(WorkspaceItem item)
		{
			this.item = item;
		}

		private WorkspaceItem item;
	}
}
