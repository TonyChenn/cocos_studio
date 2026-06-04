using System;
using Mono.Addins;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ResourcePanelExtensionAttribute : CustomExtensionAttribute
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002D35 File Offset: 0x00000F35
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002D3D File Offset: 0x00000F3D
		public Type ModelType { get; private set; }

		// Token: 0x0600002D RID: 45 RVA: 0x00002D46 File Offset: 0x00000F46
		public ResourcePanelExtensionAttribute()
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002D4E File Offset: 0x00000F4E
		public ResourcePanelExtensionAttribute(Type modelType)
		{
			this.ModelType = modelType;
		}
	}
}
