using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace CocoStudio.Core.Events
{
	public class AddResourcesArgs : EventArgs
	{
		public ResourceFolder Parent { get; private set; }

		public bool IsExpand { get; private set; }

		public IEnumerable<ResourceItem> AddItems { get; private set; }

		public AddResourcesArgs(ResourceFolder parent, IEnumerable<ResourceItem> addItems, bool isExpand = true)
		{
			this.Parent = parent;
			this.AddItems = addItems;
			this.IsExpand = isExpand;
		}

		public AddResourcesArgs(ResourceFolder parent, ResourceItem addItem, bool isExpand = true)
		{
			this.Parent = parent;
			this.AddItems = new List<ResourceItem>
			{
				addItem
			};
			this.IsExpand = isExpand;
		}
	}
}
