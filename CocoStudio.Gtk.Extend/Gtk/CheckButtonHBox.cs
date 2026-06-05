using System;
using Gdk;

namespace Gtk
{
	// Token: 0x0200005C RID: 92
	public class CheckButtonHBox : HBox
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00008AC0 File Offset: 0x00006CC0
		protected override void OnSizeAllocated(Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			Widget widget = base.Children[0];
			if (widget != null)
			{
				Rectangle allocation2 = widget.Allocation;
				allocation2.X -= 4;
				widget.Allocation = allocation2;
			}
		}
	}
}
