using System;
using Gtk;

namespace Stetic
{
	// Token: 0x0200001F RID: 31
	internal class Gui
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00006FA0 File Offset: 0x000051A0
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000035 RID: 53
		private static bool initialized;
	}
}
