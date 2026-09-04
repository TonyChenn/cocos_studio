using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200000B RID: 11
	internal class Gui
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00002B7D File Offset: 0x00000D7D
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000016 RID: 22
		private static bool initialized;
	}
}
