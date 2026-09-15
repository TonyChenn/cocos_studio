using System;
using CocoStudio.Basic;
using CocoStudio.Model;

namespace CocoStudio.Projects
{
	internal class ResourceChangeService
	{
		public static ResourceChangeService Instance { get; private set; } = new ResourceChangeService();

		private ResourceChangeService()
		{
		}

		internal void Register(ResourceItem changedResource, bool isDelete = false, ResourceData oldResourceData = null)
		{
			if (oldResourceData == null)
			{
				oldResourceData = changedResource.GetResourceData();
			}
			if (isDelete)
			{
				changedResource = null;
			}
			if (this.changedResources == null)
			{
				this.changedResources = new ChangedResourceCollection(oldResourceData, changedResource as ResourceFile);
				return;
			}
			if (!this.changedResources.ContainsKey(oldResourceData))
			{
				this.changedResources.Add(oldResourceData, changedResource as ResourceFile);
			}
		}

		public void NotifyResourceChanged()
		{
			try
			{
				if (this.changedResources != null && this.changedResources.Count != 0)
				{
					ProjectsService.Instance.NotifyResourceFileChanged(this.changedResources);
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
			}
			finally
			{
				if (this.changedResources != null)
				{
					this.changedResources.Clear();
				}
			}
		}

		private ChangedResourceCollection changedResources;
	}
}
