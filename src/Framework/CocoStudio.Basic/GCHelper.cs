using System;

namespace CocoStudio.Basic
{
	public class GCHelper
	{
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
