using System;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000008 RID: 8
	public class EditorInfo
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022A4 File Offset: 0x000004A4
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000022BB File Offset: 0x000004BB
		public string Type { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000022C4 File Offset: 0x000004C4
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000022DB File Offset: 0x000004DB
		public Version Version { get; private set; }

		// Token: 0x0600001B RID: 27 RVA: 0x000022E4 File Offset: 0x000004E4
		public EditorInfo(string type, Version version)
		{
			this.Type = type;
			this.Version = version;
		}
	}
}
