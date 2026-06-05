using System;
using Cairo;
using Gdk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x02000089 RID: 137
	public class CellRendererToggleImage : CellRendererToggle
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000C054 File Offset: 0x0000A254
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000C06B File Offset: 0x0000A26B
		public Xwt.Drawing.Image CheckedImage { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000C074 File Offset: 0x0000A274
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000C08B File Offset: 0x0000A28B
		public Xwt.Drawing.Image UnCheckedImage { get; set; }

		// Token: 0x060002F6 RID: 758 RVA: 0x0000C094 File Offset: 0x0000A294
		public CellRendererToggleImage()
		{
			base.Toggled += this.HandleToggled;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000C0B2 File Offset: 0x0000A2B2
		private void HandleToggled(object o, ToggledArgs args)
		{
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
		protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			Xwt.Drawing.Image image = this.GetImage();
			if (image != null)
			{
				using (Cairo.Context context = CairoHelper.Create(window))
				{
					int num = cell_area.X + cell_area.Width / 2 - (int)(image.Width / 2.0);
					int num2 = cell_area.Y + cell_area.Height / 2 - (int)(image.Height / 2.0);
					context.DrawImage(widget, image, (double)num, (double)num2);
				}
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000C160 File Offset: 0x0000A360
		protected void GetImageInfo(Gdk.Rectangle cell_area, out Xwt.Drawing.Image img, out int x, out int y)
		{
			img = this.GetImage();
			if (img == null)
			{
				x = cell_area.X + cell_area.Width / 2;
				y = cell_area.Y + cell_area.Height / 2;
			}
			else
			{
				x = cell_area.X + cell_area.Width / 2 - (int)(img.Width / 2.0);
				y = cell_area.Y + cell_area.Height / 2 - (int)(img.Height / 2.0);
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		public override void GetSize(Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
		{
			Xwt.Drawing.Image image = this.GetImage();
			if (image != null)
			{
				width = (int)image.Width;
				height = (int)image.Height;
			}
			else
			{
				width = (height = 0);
			}
			width += (int)(base.Xpad * 2U);
			height += (int)(base.Ypad * 2U);
			x_offset = (y_offset = 0);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000C260 File Offset: 0x0000A460
		private Xwt.Drawing.Image GetImage()
		{
			Xwt.Drawing.Image image;
			if (base.Active)
			{
				image = this.CheckedImage;
			}
			else
			{
				image = this.UnCheckedImage;
			}
			return (image != CellRendererImage.NullImage) ? image : null;
		}
	}
}
