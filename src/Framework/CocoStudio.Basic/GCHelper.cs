using System;

namespace CocoStudio.Basic
{
	// Token: 0x02000009 RID: 9
	public class GCHelper
	{
		// Token: 0x06000028 RID: 40 RVA: 0x000025FC File Offset: 0x000007FC
		public static void QuickCollect()
		{
			long totalMemory = GC.GetTotalMemory(false);
			if (totalMemory > 524288000L)
			{
				GC.Collect(0, GCCollectionMode.Forced, false);
			}
		}
	}
}
