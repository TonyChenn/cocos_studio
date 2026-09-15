using System;
using Gdk;

namespace Gtk
{
	public class BorderEventBox : EventBox
	{
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
