using System;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;

namespace Cocos.Launcher.Core
{
	public class SearchPopover : PopoverWindow
	{
		public SearchPopover(Widget parentView)
		{
			this.parentView = parentView;
			this.Initialize();
		}

		private void Initialize()
		{
			base.SkipPagerHint = true;
			base.SkipTaskbarHint = true;
			base.AllowGrow = false;
			base.AllowShrink = false;
			base.AppPaintable = false;
			base.TypeHint = WindowTypeHint.PopupMenu;
			base.TransientFor = Services.MainWindow;
			base.ShowArrow = false;
			base.WidthRequest = 190;
			base.Theme.CornerRadius = 0;
			base.Theme.SetFlatColor(new Cairo.Color(1.0, 1.0, 1.0));
			base.Theme.BorderColor = new Cairo.Color(0.8666666666666667, 0.8666666666666667, 0.8666666666666667);
			base.Theme.Padding = 1;
		}

		internal void SetContent(Widget widget)
		{
			if (this.childWidget == widget)
			{
				return;
			}
			this.childWidget = widget;
			base.ContentBox.RemoveAll();
			base.ContentBox.Add(widget);
		}

		public void ShowPopover()
		{
			base.ShowPopup(this.parentView, new Gdk.Rectangle(95, 30, 0, 0), PopupPosition.Top);
		}

		private Widget parentView;

		private Widget childWidget;
	}
}
