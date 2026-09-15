using System;
using Cairo;
using Gdk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Gtk
{
	public class CellRendererToggleImage : CellRendererToggle
	{
		public Xwt.Drawing.Image CheckedImage { get; set; }

		public Xwt.Drawing.Image UnCheckedImage { get; set; }

		public CellRendererToggleImage()
		{
			base.Toggled += this.HandleToggled;
		}

		private void HandleToggled(object o, ToggledArgs args)
		{
		}

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
