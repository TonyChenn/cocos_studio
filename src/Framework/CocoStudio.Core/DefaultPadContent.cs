using System;
using Gdk;
using Gtk;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	public class DefaultPadContent : AbstractPadContent
	{
		protected DefaultPadContent()
		{
			this.mainVbox = new VBox();
			EventBox eventBox = new EventBox();
			eventBox.HeightRequest = 1;
			eventBox.SetNormalBg(WindowStyle.LineDarkColor);
			this.mainVbox.PackStart(eventBox, false, false, 0U);
		}

		public DefaultPadContent(Widget content) : this()
		{
			this.mainVbox.PackStart(content);
			this.mainVbox.ShowAll();
			content.EnterNotifyEvent += this.HandleMouseEntered;
		}

		protected void HandleMouseEntered(object o, EnterNotifyEventArgs args)
		{
			Widget widget = o as Widget;
			if (widget != null && widget.GdkWindow != null)
			{
				this.SetToDefaultCursor(this.GetRootWindow(widget.GdkWindow));
			}
		}

		private Gdk.Window GetRootWindow(Gdk.Window wnd)
		{
			Gdk.Window window = wnd;
			while (window.Parent != null)
			{
				window = window.Parent;
			}
			return window;
		}

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

		public override Widget Control
		{
			get
			{
				return this.mainVbox;
			}
		}

		public override void Initialize(IPadWindow window)
		{
			base.Initialize(window);
		}

		public override void RedrawContent()
		{
		}

		public override void Dispose()
		{
			this.mainVbox = null;
		}

		private VBox mainVbox;
	}
}
