using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000013 RID: 19
	internal class Gui
	{
		// Token: 0x0600006D RID: 109 RVA: 0x000040E8 File Offset: 0x000022E8
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000030 RID: 48
		private static bool initialized;
	}
}
