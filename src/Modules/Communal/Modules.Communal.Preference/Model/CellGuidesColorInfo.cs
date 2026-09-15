using System;
using Cairo;
using Gdk;
using GLib;
using Gtk;

namespace Modules.Communal.Preference.Model
{
	public class CellGuidesColorInfo : CellRendererText
	{
		[Property("Cell_Guides_Data")]
		public object CellGuidesData { get; set; }

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
