using System;
using System.Drawing;

namespace Gtk.Controls
{
	// Token: 0x02000015 RID: 21
	public class ColorExEvent : EventArgs
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000449C File Offset: 0x0000269C
		// (set) Token: 0x0600009F RID: 159 RVA: 0x000044B3 File Offset: 0x000026B3
		public Color Color { get; set; }

		// Token: 0x060000A0 RID: 160 RVA: 0x000044BC File Offset: 0x000026BC
		public ColorExEvent(Color color)
		{
			this.Color = color;
		}
	}
}
