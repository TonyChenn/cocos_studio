using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000009 RID: 9
	internal class Gui
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00002D73 File Offset: 0x00000F73
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x0400001C RID: 28
		private static bool initialized;
	}
}
