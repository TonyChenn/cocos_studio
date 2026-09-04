using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000D RID: 13
	internal class Gui
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00003A51 File Offset: 0x00001C51
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x0400002A RID: 42
		private static bool initialized;
	}
}
