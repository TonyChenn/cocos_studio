using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using Mono.Addins;

namespace CocoStudio.Projects
{
	internal class CocosItemBindingManager
	{
		public List<ICocosItemBinding> ProjectBindings { get; protected set; }

		public CocosItemBindingManager()
		{
			this.ProjectBindings = new List<ICocosItemBinding>();
			this.CreateBindingList();
		}

		private void CreateBindingList()
		{
			try
			{
				ICocosItemBinding[] extensionObjects = AddinManager.GetExtensionObjects<ICocosItemBinding>();
				this.ProjectBindings.AddRange(extensionObjects);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Create CocosItem binding list failed.", exception);
			}
		}
	}
}
