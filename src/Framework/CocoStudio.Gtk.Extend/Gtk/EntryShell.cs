using System;
using System.Drawing;
using Cairo;
using Gdk;
using MonoDevelop.Core;

namespace Gtk
{
	public class EntryShell : BorderEventBox
	{
		public Entry InnerEntry { get; private set; }

		public EntryShell(Entry entry)
		{
			this.InnerEntry = entry;
			if (this.InnerEntry == null)
			{
				this.InnerEntry = new Entry();
			}
			this.InnerEntry.Name = "NoBorderEntry";
			this.InnerEntry.WidthRequest = 10;
			this.InnerEntry.HeightRequest = (Platform.IsWindows ? 23 : 22);
			this.InnerEntry.FocusInEvent += this.EntryFocusInEventHandler;
			this.InnerEntry.FocusOutEvent += this.EntryFocusOutEventHandler;
			base.HeightRequest = (Platform.IsWindows ? 25 : 24);
			base.ModifyBg(StateType.Normal, EntryShell.bgColorNormal);
			base.ModifyBg(StateType.Insensitive, EntryShell.bgColorInsensitive);
			base.ButtonPressEvent += this.BorderButtonPressEventHandler;
			this.OnCreate();
		}

		protected virtual void OnCreate()
		{
			base.Add(this.InnerEntry);
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			bool result = base.OnExposeEvent(evnt);
			System.Drawing.Color color;
			if (base.State == StateType.Insensitive)
			{
				color = EntryShell.borderColorInsensitive;
			}
			else if (this.isPrelight)
			{
				color = EntryShell.borderColorPrelight;
			}
			else
			{
				color = EntryShell.borderColorNormal;
			}
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				Gdk.Rectangle allocation = base.Allocation;
				context.LineWidth = 1.0;
				context.Antialias = Antialias.None;
				context.SetColor(color);
				if (Platform.IsWindows)
				{
					context.MoveTo(1.0, 1.0);
					context.LineTo((double)allocation.Width, 1.0);
					context.LineTo((double)allocation.Width, (double)allocation.Height);
					context.LineTo(1.0, (double)allocation.Height);
					context.LineTo(1.0, 0.0);
				}
				else
				{
					context.MoveTo(0.0, 1.0);
					context.LineTo((double)(allocation.Width - 1), 1.0);
					context.LineTo((double)(allocation.Width - 1), (double)allocation.Height);
					context.LineTo(0.0, (double)allocation.Height);
					context.LineTo(0.0, 0.0);
				}
				context.Stroke();
			}
			return result;
		}

		private void EntryFocusInEventHandler(object o, FocusInEventArgs args)
		{
			this.isPrelight = true;
			base.QueueDraw();
		}

		private void EntryFocusOutEventHandler(object o, FocusOutEventArgs args)
		{
			this.isPrelight = false;
			base.QueueDraw();
		}

		private void BorderButtonPressEventHandler(object o, ButtonPressEventArgs args)
		{
			this.InnerEntry.HasFocus = true;
			this.InnerEntry.Position = this.InnerEntry.Text.Length;
			args.RetVal = true;
		}

		protected static readonly Gdk.Color bgColorNormal = new Gdk.Color(50, 50, 54);

		protected static readonly Gdk.Color bgColorInsensitive = new Gdk.Color(62, 62, 66);

		protected static readonly System.Drawing.Color borderColorNormal = System.Drawing.Color.FromArgb(33, 33, 35);

		protected static readonly System.Drawing.Color borderColorInsensitive = System.Drawing.Color.FromArgb(44, 44, 46);

		private static readonly System.Drawing.Color borderColorPrelight = System.Drawing.Color.FromArgb(8, 114, 245);

		private bool isPrelight = false;
	}
}
