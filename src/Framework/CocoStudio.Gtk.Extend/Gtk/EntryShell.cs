using System;
using System.Drawing;
using Cairo;
using Gdk;
using MonoDevelop.Core;

namespace Gtk
{
	// Token: 0x0200005D RID: 93
	public class EntryShell : BorderEventBox
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00008B74 File Offset: 0x00006D74
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00008B8B File Offset: 0x00006D8B
		public Entry InnerEntry { get; private set; }

		// Token: 0x060001F2 RID: 498 RVA: 0x00008B94 File Offset: 0x00006D94
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

		// Token: 0x060001F3 RID: 499 RVA: 0x00008C87 File Offset: 0x00006E87
		protected virtual void OnCreate()
		{
			base.Add(this.InnerEntry);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008C98 File Offset: 0x00006E98
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

		// Token: 0x060001F5 RID: 501 RVA: 0x00008E68 File Offset: 0x00007068
		private void EntryFocusInEventHandler(object o, FocusInEventArgs args)
		{
			this.isPrelight = true;
			base.QueueDraw();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008E79 File Offset: 0x00007079
		private void EntryFocusOutEventHandler(object o, FocusOutEventArgs args)
		{
			this.isPrelight = false;
			base.QueueDraw();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008E8A File Offset: 0x0000708A
		private void BorderButtonPressEventHandler(object o, ButtonPressEventArgs args)
		{
			this.InnerEntry.HasFocus = true;
			this.InnerEntry.Position = this.InnerEntry.Text.Length;
			args.RetVal = true;
		}

		// Token: 0x040002FA RID: 762
		protected static readonly Gdk.Color bgColorNormal = new Gdk.Color(50, 50, 54);

		// Token: 0x040002FB RID: 763
		protected static readonly Gdk.Color bgColorInsensitive = new Gdk.Color(62, 62, 66);

		// Token: 0x040002FC RID: 764
		protected static readonly System.Drawing.Color borderColorNormal = System.Drawing.Color.FromArgb(33, 33, 35);

		// Token: 0x040002FD RID: 765
		protected static readonly System.Drawing.Color borderColorInsensitive = System.Drawing.Color.FromArgb(44, 44, 46);

		// Token: 0x040002FE RID: 766
		private static readonly System.Drawing.Color borderColorPrelight = System.Drawing.Color.FromArgb(8, 114, 245);

		// Token: 0x040002FF RID: 767
		private bool isPrelight = false;
	}
}
