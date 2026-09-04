using System;
using Gdk;
using GLib;

namespace Gtk
{
	// Token: 0x02000017 RID: 23
	public class CustomExpender : Expander
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x0000458C File Offset: 0x0000278C
		public CustomExpender(string label) : base(label)
		{
			base.BorderWidth = 3U;
			base.WidgetEvent += this.CustomExpender_WidgetEvent;
			base.SizeAllocated += this.CustomExpender_SizeAllocated;
			base.ExposeEvent += this.CustomExpender_ExposeEvent;
			base.Destroyed += this.CustomExpender_Destroyed;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004600 File Offset: 0x00002800
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

		// Token: 0x060000A8 RID: 168 RVA: 0x00004684 File Offset: 0x00002884
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

		// Token: 0x060000A9 RID: 169 RVA: 0x00004708 File Offset: 0x00002908
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

		// Token: 0x060000AA RID: 170 RVA: 0x00004788 File Offset: 0x00002988
		private void CustomExpender_SizeAllocated(object o, SizeAllocatedArgs args)
		{
			if (base.Parent != null && base.Parent.Parent != null)
			{
				base.Parent.Parent.QueueDraw();
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000047C8 File Offset: 0x000029C8
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

		// Token: 0x060000AC RID: 172 RVA: 0x00004890 File Offset: 0x00002A90
		private bool IsClickTitle(EventButton evnt)
		{
			Rectangle originTitleSize = this.GetOriginTitleSize();
			return originTitleSize.Width != 0 && originTitleSize.Contains((int)evnt.XRoot, (int)evnt.YRoot);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000048CC File Offset: 0x00002ACC
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

		// Token: 0x060000AE RID: 174 RVA: 0x000049A0 File Offset: 0x00002BA0
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

		// Token: 0x060000AF RID: 175 RVA: 0x00004A54 File Offset: 0x00002C54
		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			return !this.IsClickTitle(evnt) && base.OnButtonPressEvent(evnt);
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000B0 RID: 176 RVA: 0x00004A7C File Offset: 0x00002C7C
		// (remove) Token: 0x060000B1 RID: 177 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public event EventHandler<ExpandEvent> ExpandChanged;

		// Token: 0x0400003F RID: 63
		private EventBox parentEventBox;

		// Token: 0x04000040 RID: 64
		private bool isloaded = false;
	}
}
