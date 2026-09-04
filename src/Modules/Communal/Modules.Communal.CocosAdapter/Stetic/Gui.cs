using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000027 RID: 39
	internal class Gui
	{
		// Token: 0x06000149 RID: 329 RVA: 0x000063ED File Offset: 0x000045ED
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000079 RID: 121
		private static bool initialized;
	}
}
