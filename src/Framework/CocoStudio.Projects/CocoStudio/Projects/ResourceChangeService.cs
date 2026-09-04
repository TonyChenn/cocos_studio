using System;
using CocoStudio.Basic;
using CocoStudio.Model;

namespace CocoStudio.Projects
{
	// Token: 0x0200008C RID: 140
	internal class ResourceChangeService
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000E121 File Offset: 0x0000C321
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x0000E128 File Offset: 0x0000C328
		public static ResourceChangeService Instance { get; private set; } = new ResourceChangeService();

		// Token: 0x06000452 RID: 1106 RVA: 0x0000E13C File Offset: 0x0000C33C
		private ResourceChangeService()
		{
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000E144 File Offset: 0x0000C344
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

		// Token: 0x06000454 RID: 1108 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
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

		// Token: 0x04000123 RID: 291
		private ChangedResourceCollection changedResources;
	}
}
