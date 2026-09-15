using System;
using System.Collections.Generic;

namespace CocoStudio.Projects
{
	public class ResourceInfoDragData
	{
		public IList<ResourceItem> Items { get; private set; }

		public ResourceInfoDragData(IList<ResourceItem> ResourceList)
		{
			this.Items = ResourceList;
		}
	}
}
