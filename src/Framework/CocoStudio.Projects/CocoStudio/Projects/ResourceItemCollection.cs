using System;

namespace CocoStudio.Projects
{
	public class ResourceItemCollection : ItemCollection<ResourceItem>
	{
		private ResourceItemCollection()
		{
		}

		public ResourceItemCollection(ResourceItem parentItem)
		{
			this.parentItem = parentItem;
		}

		protected override void OnAdd(ResourceItem item)
		{
			item.Parent = this.parentItem;
		}

		protected override void OnRemove(ResourceItem item)
		{
			item.Parent = null;
		}

		private ResourceItem parentItem;
	}
}
