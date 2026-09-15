using System;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Components;

namespace Modules.Communal.ResourcePanel
{
	public class CustomCellRendererImage : CellRendererImage
	{
		[Property("user-cell-data")]
		public object UserCellData { get; set; }

		protected override void Render(Drawable window, Widget widget, Rectangle background_area, Rectangle cell_area, Rectangle expose_area, CellRendererState flags)
		{
			CellRenderHelper.Render(this.UserCellData, window, widget, background_area, cell_area, expose_area, flags);
			base.Render(window, widget, background_area, cell_area, expose_area, flags);
		}
	}
}
