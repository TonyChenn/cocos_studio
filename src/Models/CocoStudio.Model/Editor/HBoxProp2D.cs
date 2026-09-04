using System;
using System.Collections.Generic;
using Gdk;
using Gtk;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000059 RID: 89
	public class HBoxProp2D : HBox
	{
		// Token: 0x06000316 RID: 790 RVA: 0x0000C810 File Offset: 0x0000AA10
		protected override void OnSizeAllocated(Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			List<Widget> list = new List<Widget>();
			foreach (Widget widget in base.Children)
			{
				if (widget.Visible)
				{
					list.Add(widget);
				}
			}
			if (list.Count > 1 && list.Count < 4)
			{
				Rectangle allocation2 = allocation;
				if (list.Count == 2)
				{
					int num = (allocation.Width - 4) / 2;
					allocation2.X = base.Allocation.X;
					allocation2.Width = num;
					list[0].SizeAllocate(allocation2);
					allocation2.X = num + 4 + base.Allocation.X;
					list[1].SizeAllocate(allocation2);
				}
				else if (list.Count == 3)
				{
					int num = (allocation.Width - 4 - 20) / 2;
					allocation2.X = base.Allocation.X;
					allocation2.Width = num;
					list[0].SizeAllocate(allocation2);
					allocation2.X = num + 4 + 20 + base.Allocation.X;
					list[2].SizeAllocate(allocation2);
					allocation2.X = num + 4 + base.Allocation.X;
					allocation2.Width = 20;
					list[1].SizeAllocate(allocation2);
				}
			}
		}

		// Token: 0x04000171 RID: 369
		private const int middleSpacing = 4;

		// Token: 0x04000172 RID: 370
		private const int middleWidth = 20;
	}
}
