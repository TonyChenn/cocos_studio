using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000056 RID: 86
	internal class Gui
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x0000868C File Offset: 0x0000688C
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x040002F7 RID: 759
		private static bool initialized;
	}
}
