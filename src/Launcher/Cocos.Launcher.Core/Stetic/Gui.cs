using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000E RID: 14
	internal class Gui
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00004ACE File Offset: 0x00002CCE
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000046 RID: 70
		private static bool initialized;
	}
}
