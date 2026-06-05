using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200014F RID: 335
	public class BuildData
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0002DE89 File Offset: 0x0002C089
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x0002DE91 File Offset: 0x0002C091
		public ProjectItemCollection Items { get; internal set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0002DE9A File Offset: 0x0002C09A
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x0002DEA2 File Offset: 0x0002C0A2
		public DotNetProjectConfiguration Configuration { get; internal set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0002DEAB File Offset: 0x0002C0AB
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x0002DEB3 File Offset: 0x0002C0B3
		public ConfigurationSelector ConfigurationSelector { get; internal set; }
	}
}
