using System;
using Gtk;

namespace Stetic
{
	// Token: 0x02000016 RID: 22
	internal class Gui
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00003BF8 File Offset: 0x00001DF8
		internal static void Initialize(Widget iconRenderer)
		{
			if (!Gui.initialized)
			{
				Gui.initialized = true;
			}
		}

		// Token: 0x04000073 RID: 115
		private static bool initialized;
	}
}
