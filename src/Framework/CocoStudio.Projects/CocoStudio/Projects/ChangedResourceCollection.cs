using System;
using System.Collections.Generic;
using CocoStudio.Model;

namespace CocoStudio.Projects
{
	public class ChangedResourceCollection : Dictionary<ResourceData, ResourceFile>
	{
		public ChangedResourceCollection(ResourceData oldData, ResourceItem resourceFile)
		{
			base.Add(oldData, resourceFile as ResourceFile);
		}
	}
}
