using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000003 RID: 3
	internal class Gui
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002150 File Offset: 0x00000350
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000004 RID: 4
		private static bool initialized;
	}
}
