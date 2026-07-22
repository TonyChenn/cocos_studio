using System;
using System.Drawing;
using Cairo;
using CocoStudio.Projects;
using Gdk;
using Gtk;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000002 RID: 2
	public static class CellRenderHelper
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static object HoverdItem { get; set; }

		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
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

		// Token: 0x06000004 RID: 4 RVA: 0x00002108 File Offset: 0x00000308
		public static void SetColor(this Context cr, System.Drawing.Color color)
		{
			cr.SetSourceRGBA((double)color.R / 255.0, (double)color.G / 255.0, (double)color.B / 255.0, (double)color.A / 255.0);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002163 File Offset: 0x00000363
		public static void SetColor(this Context cr, Gdk.Color color)
		{
			cr.SetSourceRGB((double)color.Red / 255.0, (double)color.Green / 255.0, (double)color.Blue / 255.0);
		}

		// Token: 0x04000001 RID: 1
		public static readonly System.Drawing.Color ResourceTree_Background = System.Drawing.Color.FromArgb(255, 65, 65, 70);

		// Token: 0x04000002 RID: 2
		public static readonly System.Drawing.Color ResourceTree_Node_Background = System.Drawing.Color.FromArgb(255, 65, 65, 70);

		// Token: 0x04000003 RID: 3
		public static readonly System.Drawing.Color ResourceTree_Node_Selected = System.Drawing.Color.FromArgb(255, 8, 114, 245);

		// Token: 0x04000004 RID: 4
		public static readonly System.Drawing.Color Resource_node_Hoverd = System.Drawing.Color.FromArgb(100, 8, 114, 245);
	}
}
