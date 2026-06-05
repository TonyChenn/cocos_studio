using System;
using GLib;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200001B RID: 27
	public class GtkInvokeHelp
	{
		// Token: 0x06000170 RID: 368 RVA: 0x000068B0 File Offset: 0x00004AB0
		public static void BeginInvoke(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			Timeout.Add(500U, delegate
			{
				action();
				return false;
			});
		}

		// Token: 0x0400001E RID: 30
		private const int timeSpan = 500;
	}
}
