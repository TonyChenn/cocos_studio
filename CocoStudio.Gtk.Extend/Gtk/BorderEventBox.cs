using System;
using Gdk;

namespace Gtk
{
	// Token: 0x0200005B RID: 91
	public class BorderEventBox : EventBox
	{
		// Token: 0x060001EB RID: 491 RVA: 0x00008A08 File Offset: 0x00006C08
		protected override void OnSizeAllocated(Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			Widget child = base.Child;
			if (child != null)
			{
				Rectangle allocation2 = base.Child.Allocation;
				allocation2.X++;
				allocation2.Y++;
				allocation2.Width -= 2;
				allocation2.Height -= 2;
				if (allocation2.Width < 0)
				{
					allocation2.Width = 0;
				}
				if (allocation2.Height < 0)
				{
					allocation2.Height = 0;
				}
				child.Allocation = allocation2;
			}
		}
	}
}
