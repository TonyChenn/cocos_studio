using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000002 RID: 2
	internal class Gui
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000001 RID: 1
		private static bool initialized;
	}
}
