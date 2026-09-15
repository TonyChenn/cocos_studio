using System;
using Gdk;
using GLib;

namespace Gtk
{
	public class CustomExpender : Expander
	{
		public CustomExpender(string label) : base(label)
		{
			base.BorderWidth = 3U;
			base.WidgetEvent += this.CustomExpender_WidgetEvent;
			base.SizeAllocated += this.CustomExpender_SizeAllocated;
			base.ExposeEvent += this.CustomExpender_ExposeEvent;
			base.Destroyed += this.CustomExpender_Destroyed;
		}

		private void CustomExpender_Destroyed(object sender, EventArgs e)
		{
			base.Destroyed -= this.CustomExpender_Destroyed;
			base.WidgetEvent -= this.CustomExpender_WidgetEvent;
			base.SizeAllocated -= this.CustomExpender_SizeAllocated;
			base.ExposeEvent -= this.CustomExpender_ExposeEvent;
			if (this.parentEventBox != null)
			{
				this.parentEventBox.WidgetEvent -= this.Parent_WidgetEvent;
			}
		}

		private void CustomExpender_WidgetEvent(object o, WidgetEventArgs args)
		{
			if (args.Event.ToString() == "Gdk.EventExpose")
			{
				if (!this.isloaded && base.Parent != null)
				{
					this.parentEventBox = this.GetParentWidget<EventBox>();
					if (this.parentEventBox != null)
					{
						this.parentEventBox.WidgetEvent += this.Parent_WidgetEvent;
						this.isloaded = true;
					}
				}
			}
		}

		[ConnectBefore]
		private void Parent_WidgetEvent(object o, WidgetEventArgs args)
		{
			EventType type = args.Event.Type;
			if (type == EventType.ButtonPress)
			{
				EventButton evnt = (EventButton)args.Event;
				if (this.IsClickTitle(evnt))
				{
					base.Expanded = !base.Expanded;
					if (this.ExpandChanged != null)
					{
						this.ExpandChanged(this, new ExpandEvent(base.Label, base.Expanded));
					}
				}
			}
		}

		private void CustomExpender_SizeAllocated(object o, SizeAllocatedArgs args)
		{
			if (base.Parent != null && base.Parent.Parent != null)
			{
				base.Parent.Parent.QueueDraw();
			}
		}

		private void CustomExpender_ExposeEvent(object o, ExposeEventArgs args)
		{
			Rectangle titleSize = this.GetTitleSize();
			if (titleSize.Width != 0)
			{
				Gdk.GC gc = new Gdk.GC(base.GdkWindow);
				gc.RgbFgColor = WindowStyle.WindowLineColor;
				base.Parent.Parent.GdkWindow.DrawLine(gc, titleSize.X, titleSize.Y, titleSize.X + titleSize.Width, titleSize.Y);
				base.Parent.Parent.GdkWindow.DrawLine(gc, titleSize.X, titleSize.Y + titleSize.Height, titleSize.X + titleSize.Width, titleSize.Y + titleSize.Height);
			}
		}

		private bool IsClickTitle(EventButton evnt)
		{
			Rectangle originTitleSize = this.GetOriginTitleSize();
			return originTitleSize.Width != 0 && originTitleSize.Contains((int)evnt.XRoot, (int)evnt.YRoot);
		}

		private Rectangle GetTitleSize()
		{
			Rectangle result;
			if (base.Parent != null && base.Parent.Parent != null)
			{
				int width = base.Parent.Parent.Allocation.Width;
				int borderWidth = (int)base.BorderWidth;
				int x = 0;
				int num = base.Allocation.Y + borderWidth;
				int width2 = width;
				int num2 = base.Allocation.Y + base.LabelWidget.Allocation.Height + borderWidth * 2;
				int num3 = base.Allocation.Y + base.LabelWidget.Allocation.Height + borderWidth * 2;
				result = new Rectangle(x, num, width2, num2 - num);
			}
			else
			{
				result = default(Rectangle);
			}
			return result;
		}

		private Rectangle GetOriginTitleSize()
		{
			int num = -1;
			int num2 = -1;
			int borderWidth = (int)base.BorderWidth;
			Rectangle result;
			if (this.parentEventBox != null && base.GdkWindow != null && base.GdkWindow.GetOrigin(out num, out num2) > 0)
			{
				num += base.Allocation.Left;
				num2 += base.Allocation.Top + borderWidth;
				int width = this.parentEventBox.Allocation.Width;
				int height = base.LabelWidget.Allocation.Height + borderWidth;
				result = new Rectangle(num, num2, width, height);
			}
			else
			{
				result = default(Rectangle);
			}
			return result;
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			return !this.IsClickTitle(evnt) && base.OnButtonPressEvent(evnt);
		}

		public event EventHandler<ExpandEvent> ExpandChanged;

		private EventBox parentEventBox;

		private bool isloaded = false;
	}
}
