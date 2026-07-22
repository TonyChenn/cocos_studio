using System;
using System.Drawing;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Core;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200001B RID: 27
	internal class TitleMainEventBox : EventBox
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00004474 File Offset: 0x00002674
		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			Widget child = base.Child;
			if (child != null)
			{
				Gdk.Rectangle allocation2 = base.Child.Allocation;
				if (allocation2.Width != 1 || allocation2.Height != 1)
				{
					allocation2.X += 15;
					allocation2.Width = allocation2.Width - 15 - 10;
					allocation2.Y += 5;
					allocation2.Height = allocation2.Height - 5 - 5 - 1;
					child.Allocation = allocation2;
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000451A File Offset: 0x0000271A
		protected override void OnSizeRequested(ref Requisition requisition)
		{
			base.OnSizeRequested(ref requisition);
			requisition.Width = requisition.Width + 15 + 10;
			requisition.Height = requisition.Height + 5 + 5 + 1;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000454C File Offset: 0x0000274C
		protected override bool OnExposeEvent(EventExpose evnt)
		{
			bool result = base.OnExposeEvent(evnt);
			System.Drawing.Color color = System.Drawing.Color.FromArgb(33, 33, 35);
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				Gdk.Rectangle allocation = base.Allocation;
				context.LineWidth = 1.0;
				context.Antialias = Antialias.None;
				context.SetColor(color);
				if (Platform.IsWindows)
				{
					context.MoveTo(1.0, (double)allocation.Height);
					context.LineTo((double)allocation.Width, (double)allocation.Height);
				}
				else
				{
					context.MoveTo(0.0, (double)allocation.Height);
					context.LineTo((double)(allocation.Width - 1), (double)allocation.Height);
				}
				context.Stroke();
			}
			return result;
		}

		// Token: 0x04000035 RID: 53
		private const int leftPadding = 15;

		// Token: 0x04000036 RID: 54
		private const int rightPadding = 10;

		// Token: 0x04000037 RID: 55
		private const int topPadding = 5;

		// Token: 0x04000038 RID: 56
		private const int bottomPadding = 5;
	}
}
