using System.ComponentModel;
using Cairo;
using Gdk;
using Gtk;

namespace MonoDevelop.Debugger
{
	[ToolboxItem(true)]
	internal class InfoFrame : Frame
	{
		public InfoFrame()
		{
			base.Shadow = ShadowType.None;
		}

		public InfoFrame(Widget child)
			: this()
		{
			base.Child = child;
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				context.Rectangle(base.Allocation.X, base.Allocation.Y, base.Allocation.Width, base.Allocation.Height);
				context.ClipPreserve();
				context.SetSourceRGB(1.0, 0.98, 0.91);
				context.FillPreserve();
				context.SetSourceRGB(0.87, 0.83, 0.74);
				context.LineWidth = 2.0;
				context.Stroke();
			}
			return base.OnExposeEvent(evnt);
		}
	}
}
