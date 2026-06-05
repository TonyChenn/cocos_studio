using System;
using Gdk;

namespace Gtk
{
	// Token: 0x02000099 RID: 153
	public class ColorSetEventArgs : EventArgs
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0000DB65 File Offset: 0x0000BD65
		public ColorSetEventArgs(Color color)
		{
			this.Color = color;
		}

		// Token: 0x040003D3 RID: 979
		public Color Color;
	}
}
