using System;
using Gdk;

namespace Gtk
{
	public class CheckButtonHBox : HBox
	{
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
