using System;
using System.Collections.Generic;
using Mono.Addins;

namespace CocoStudio.Projects
{
	internal class ResourceTypeManager
	{
		public List<Type> ResourceTypeList { get; private set; }

		public ResourceTypeManager()
		{
			this.ResourceTypeList = new List<Type>();
			this.CollectResourceType();
		}

		private void CollectResourceType()
		{
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes(typeof(IResource));
			foreach (object obj in extensionNodes)
			{
				TypeExtensionNode typeExtensionNode = (TypeExtensionNode)obj;
				this.ResourceTypeList.Add(typeExtensionNode.Type);
			}
		}
	}
}
