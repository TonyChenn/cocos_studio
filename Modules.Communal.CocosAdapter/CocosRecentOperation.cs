using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000007 RID: 7
	[Extension(Type = typeof(IUserData))]
	internal class CocosRecentOperation : IUserData
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000028BF File Offset: 0x00000ABF
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000028C7 File Offset: 0x00000AC7
		[ItemProperty("LastPublishType/Value")]
		public EnumPublishType LastPublishType { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000028D0 File Offset: 0x00000AD0
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000028D8 File Offset: 0x00000AD8
		[ItemProperty("IsLastPublish/Value")]
		public bool IsLastPublish { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000028E1 File Offset: 0x00000AE1
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000028E9 File Offset: 0x00000AE9
		[ItemProperty("LastRunType/Value")]
		public EnumPlatform LastRunType { get; set; }

		// Token: 0x06000030 RID: 48 RVA: 0x000028F2 File Offset: 0x00000AF2
		public CocosRecentOperation()
		{
			this.IsLastPublish = true;
			this.LastPublishType = EnumPublishType.Resource;
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				this.LastRunType = EnumPlatform.Windows;
				return;
			}
			this.LastRunType = EnumPlatform.Mac;
		}

		// Token: 0x04000001 RID: 1
		public const string userDataKey = "CocosRecentOperation";
	}
}
