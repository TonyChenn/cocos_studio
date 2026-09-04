using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200001F RID: 31
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public sealed class AssetOrderAttribute : Attribute
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006CE1 File Offset: 0x00004EE1
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00006CE9 File Offset: 0x00004EE9
		public int Order { get; set; }

		// Token: 0x06000124 RID: 292 RVA: 0x00006CF2 File Offset: 0x00004EF2
		public AssetOrderAttribute(int order)
		{
			this.Order = order;
		}
	}
}
