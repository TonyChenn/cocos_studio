using System;

namespace CocoStudio.Basic
{
	// Token: 0x02000007 RID: 7
	public static class LogConfig
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022F4 File Offset: 0x000004F4
		// (set) Token: 0x06000012 RID: 18 RVA: 0x0000230A File Offset: 0x0000050A
		public static ICSLog Logger { get; private set; } = new CSLogger(false);

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002314 File Offset: 0x00000514
		// (set) Token: 0x06000014 RID: 20 RVA: 0x0000232A File Offset: 0x0000052A
		public static ICSLog Output { get; private set; } = new CSLogger(true);

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002334 File Offset: 0x00000534
		// (set) Token: 0x06000016 RID: 22 RVA: 0x0000234A File Offset: 0x0000054A
		public static ICSLog OutputWithoutTip { get; private set; } = new CSLogger(true);

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002354 File Offset: 0x00000554
		// (set) Token: 0x06000018 RID: 24 RVA: 0x0000236A File Offset: 0x0000056A
		public static ICSLog TipWithOutConsole { get; private set; } = new CSLogger(false);

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002374 File Offset: 0x00000574
		// (set) Token: 0x0600001A RID: 26 RVA: 0x0000238A File Offset: 0x0000058A
		public static bool IsLogActivated { get; set; } = true;
	}
}
