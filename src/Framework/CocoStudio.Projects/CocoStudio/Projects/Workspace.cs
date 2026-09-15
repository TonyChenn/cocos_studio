using System;
using System.Collections.ObjectModel;

namespace CocoStudio.Projects
{
	public class Workspace : WorkspaceItem
	{
		public ObservableCollection<WorkspaceItem> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
			}
		}

		private ObservableCollection<WorkspaceItem> items;
	}
}
