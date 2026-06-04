using System;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Components;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000003 RID: 3
	public class CustomCellRendererImage : CellRendererImage
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002206 File Offset: 0x00000406
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000220E File Offset: 0x0000040E
		[Property("user-cell-data")]
		public object UserCellData { get; set; }

		// Token: 0x06000009 RID: 9 RVA: 0x00002217 File Offset: 0x00000417
		protected override void Render(Drawable window, Widget widget, Rectangle background_area, Rectangle cell_area, Rectangle expose_area, CellRendererState flags)
		{
			CellRenderHelper.Render(this.UserCellData, window, widget, background_area, cell_area, expose_area, flags);
			base.Render(window, widget, background_area, cell_area, expose_area, flags);
		}
	}
}
