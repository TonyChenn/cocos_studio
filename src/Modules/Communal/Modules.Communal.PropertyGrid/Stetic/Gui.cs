using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200001F RID: 31
	internal class Gui
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00005548 File Offset: 0x00003748
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x0400004A RID: 74
		private static bool initialized;
	}
}
