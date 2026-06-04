using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000009 RID: 9
	internal class Gui
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002EE5 File Offset: 0x000010E5
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000026 RID: 38
		private static bool initialized;
	}
}
