using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000004 RID: 4
	internal class Gui
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000218D File Offset: 0x0000038D
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000002 RID: 2
		private static bool initialized;
	}
}
