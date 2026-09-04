using System;
using Cairo;
using Gdk;
using GLib;
using Gtk;

namespace Modules.Communal.Preference.Model
{
	// Token: 0x02000007 RID: 7
	public class CellGuidesColorInfo : CellRendererText
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000218B File Offset: 0x0000038B
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002193 File Offset: 0x00000393
		[Property("Cell_Guides_Data")]
		public object CellGuidesData { get; set; }

		// Token: 0x06000015 RID: 21 RVA: 0x000021A4 File Offset: 0x000003A4
		protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			GuidesColorInfo guidesColorInfo = this.CellGuidesData as GuidesColorInfo;
			if (guidesColorInfo != null)
			{
				using (Context context = CairoHelper.Create(window))
				{
					context.SetColor(guidesColorInfo.RenderColor);
					context.Rectangle((double)(background_area.X + 2), (double)(background_area.Y + 3), 15.0, 15.0);
					context.Fill();
				}
			}
			base.Render(window, widget, background_area, cell_area, expose_area, flags);
		}
	}
}
