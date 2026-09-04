using System;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200005B RID: 91
	public class SearchPopover : PopoverWindow
	{
		// Token: 0x06000325 RID: 805 RVA: 0x0000CB75 File Offset: 0x0000AD75
		public SearchPopover(Widget parentView)
		{
			this.parentView = parentView;
			this.Initialize();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000CB8C File Offset: 0x0000AD8C
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

		// Token: 0x06000327 RID: 807 RVA: 0x0000CC4F File Offset: 0x0000AE4F
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

		// Token: 0x06000328 RID: 808 RVA: 0x0000CC79 File Offset: 0x0000AE79
		public void ShowPopover()
		{
			base.ShowPopup(this.parentView, new Gdk.Rectangle(95, 30, 0, 0), PopupPosition.Top);
		}

		// Token: 0x04000125 RID: 293
		private Widget parentView;

		// Token: 0x04000126 RID: 294
		private Widget childWidget;
	}
}
