using System;
using System.Collections.Generic;

namespace CocoStudio.Projects.Formates
{
	public class MaterialItem
	{
		public MaterialItem(string container, List<string> resourceFileList)
		{
			this.ContainerFile = container;
			this.ResourceFileList = resourceFileList;
		}

		public string ContainerFile { get; private set; }

		public List<string> ResourceFileList { get; private set; }
	}
}
