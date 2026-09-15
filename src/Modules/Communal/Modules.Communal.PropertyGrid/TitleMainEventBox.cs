using System;
using System.Drawing;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Core;

namespace Modules.Communal.PropertyGrid
{
	internal class TitleMainEventBox : EventBox
	{
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

		protected override void OnSizeRequested(ref Requisition requisition)
		{
			base.OnSizeRequested(ref requisition);
			requisition.Width = requisition.Width + 15 + 10;
			requisition.Height = requisition.Height + 5 + 5 + 1;
		}

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

		private const int leftPadding = 15;

		private const int rightPadding = 10;

		private const int topPadding = 5;

		private const int bottomPadding = 5;
	}
}
