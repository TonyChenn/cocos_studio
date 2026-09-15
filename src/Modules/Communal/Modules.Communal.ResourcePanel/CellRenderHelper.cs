using System;
using System.Drawing;
using Cairo;
using CocoStudio.Projects;
using Gdk;
using Gtk;

namespace Modules.Communal.ResourcePanel
{
	public static class CellRenderHelper
	{
		public static object HoverdItem { get; set; }

		public static void Render(object item, Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			if (item == null)
			{
				return;
			}
			using (Context context = CairoHelper.Create(window))
			{
				if ((flags & CellRendererState.Selected) != (CellRendererState)0)
				{
					context.SetColor(CellRenderHelper.ResourceTree_Node_Selected);
				}
				else if (CellRenderHelper.HoverdItem == item)
				{
					context.SetColor(CellRenderHelper.Resource_node_Hoverd);
				}
				else if (!(item is ResourceItem))
				{
					context.SetColor(CellRenderHelper.ResourceTree_Node_Background);
				}
				else
				{
					context.SetColor(CellRenderHelper.ResourceTree_Background);
				}
				context.Rectangle((double)background_area.X, (double)background_area.Y, (double)background_area.Width, (double)background_area.Height);
				context.Fill();
			}
		}

		public static void SetColor(this Context cr, System.Drawing.Color color)
		{
			cr.SetSourceRGBA((double)color.R / 255.0, (double)color.G / 255.0, (double)color.B / 255.0, (double)color.A / 255.0);
		}

		public static void SetColor(this Context cr, Gdk.Color color)
		{
			cr.SetSourceRGB((double)color.Red / 255.0, (double)color.Green / 255.0, (double)color.Blue / 255.0);
		}

		public static readonly System.Drawing.Color ResourceTree_Background = System.Drawing.Color.FromArgb(255, 65, 65, 70);

		public static readonly System.Drawing.Color ResourceTree_Node_Background = System.Drawing.Color.FromArgb(255, 65, 65, 70);

		public static readonly System.Drawing.Color ResourceTree_Node_Selected = System.Drawing.Color.FromArgb(255, 8, 114, 245);

		public static readonly System.Drawing.Color Resource_node_Hoverd = System.Drawing.Color.FromArgb(100, 8, 114, 245);
	}
}
