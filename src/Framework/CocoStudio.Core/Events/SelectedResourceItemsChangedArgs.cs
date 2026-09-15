using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	public class SelectedResourceItemsChangedArgs
	{
		public List<ResourceItem> CurrentSelectedItems { get; private set; }

		public SelectedResourceItemsChangedArgs(List<ResourceItem> currenSelectedItems)
		{
			this.CurrentSelectedItems = currenSelectedItems;
		}
	}
}
