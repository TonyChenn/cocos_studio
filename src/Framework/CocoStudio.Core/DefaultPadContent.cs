using System;
using Gdk;
using Gtk;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	// Token: 0x0200004E RID: 78
	public class DefaultPadContent : AbstractPadContent
	{
		// Token: 0x06000305 RID: 773 RVA: 0x0000E138 File Offset: 0x0000C338
		protected DefaultPadContent()
		{
			this.mainVbox = new VBox();
			EventBox eventBox = new EventBox();
			eventBox.HeightRequest = 1;
			eventBox.SetNormalBg(WindowStyle.LineDarkColor);
			this.mainVbox.PackStart(eventBox, false, false, 0U);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000E183 File Offset: 0x0000C383
		public DefaultPadContent(Widget content) : this()
		{
			this.mainVbox.PackStart(content);
			this.mainVbox.ShowAll();
			content.EnterNotifyEvent += this.HandleMouseEntered;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000E1BC File Offset: 0x0000C3BC
		protected void HandleMouseEntered(object o, EnterNotifyEventArgs args)
		{
			Widget widget = o as Widget;
			if (widget != null && widget.GdkWindow != null)
			{
				this.SetToDefaultCursor(this.GetRootWindow(widget.GdkWindow));
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000E1FC File Offset: 0x0000C3FC
		private Gdk.Window GetRootWindow(Gdk.Window wnd)
		{
			Gdk.Window window = wnd;
			while (window.Parent != null)
			{
				window = window.Parent;
			}
			return window;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000E22C File Offset: 0x0000C42C
		private void SetToDefaultCursor(Gdk.Window wnd)
		{
			if (wnd != null)
			{
				wnd.Cursor = null;
				if (wnd.Children != null)
				{
					foreach (Gdk.Window toDefaultCursor in wnd.Children)
					{
						this.SetToDefaultCursor(toDefaultCursor);
					}
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000E284 File Offset: 0x0000C484
		public override Widget Control
		{
			get
			{
				return this.mainVbox;
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000E29C File Offset: 0x0000C49C
		public override void Initialize(IPadWindow window)
		{
			base.Initialize(window);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000E2A7 File Offset: 0x0000C4A7
		public override void RedrawContent()
		{
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000E2AA File Offset: 0x0000C4AA
		public override void Dispose()
		{
			this.mainVbox = null;
		}

		// Token: 0x04000165 RID: 357
		private VBox mainVbox;
	}
}
