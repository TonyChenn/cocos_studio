using System;
using Cairo;

namespace Gtk
{
	// Token: 0x0200005A RID: 90
	public static class HelperMethods
	{
		// Token: 0x060001EA RID: 490 RVA: 0x000089E0 File Offset: 0x00006BE0
		public static void SetSourceColor(this Context cr, Color color)
		{
			cr.SetSourceRGBA(color.R, color.G, color.B, color.A);
		}
	}
}
