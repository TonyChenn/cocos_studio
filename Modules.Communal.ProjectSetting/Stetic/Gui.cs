using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000006 RID: 6
	internal class Gui
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000022A5 File Offset: 0x000004A5
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000009 RID: 9
		private static bool initialized;
	}
}
