using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000B RID: 11
	internal class Gui
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002AE0 File Offset: 0x00000CE0
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000023 RID: 35
		private static bool initialized;
	}
}
