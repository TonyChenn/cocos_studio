using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000068 RID: 104
	internal class CocosItemBindingManager
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000B4B9 File Offset: 0x000096B9
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000B4C1 File Offset: 0x000096C1
		public List<ICocosItemBinding> ProjectBindings { get; protected set; }

		// Token: 0x06000303 RID: 771 RVA: 0x0000B4CA File Offset: 0x000096CA
		public CocosItemBindingManager()
		{
			this.ProjectBindings = new List<ICocosItemBinding>();
			this.CreateBindingList();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000B4E4 File Offset: 0x000096E4
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
