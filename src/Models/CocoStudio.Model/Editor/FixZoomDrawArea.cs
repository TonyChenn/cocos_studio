using System;
using System.IO;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	public class FixZoomDrawArea : DrawingArea
	{
		public FixZoomDrawArea()
		{
			base.WidgetFlags |= (WidgetFlags.NoWindow | WidgetFlags.AppPaintable);
			this.imageH = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.position_arrow.png");
			this.imageV = this.RotationImage(this.imageH, PixbufRotation.Counterclockwise);
			this.imageBlueH = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.position_arrow_over.png");
			this.imageBlueV = this.RotationImage(this.imageBlueH, PixbufRotation.Counterclockwise);
		}

		private Xwt.Drawing.Image RotationImage(Xwt.Drawing.Image image, PixbufRotation rotation)
		{
			return Xwt.Drawing.Image.FromStream(new MemoryStream(image.GetPixbuf().RotateSimple(rotation).SaveToBuffer("png")));
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (Cairo.Context context = CairoHelper.Create(evnt.Window))
			{
				context.Save();
				context.DrawImage(this, (this.HType == 0) ? this.imageH : this.imageBlueH, 0.0, 18.0);
				context.Restore();
				context.Save();
				context.DrawImage(this, (this.VType == 0) ? this.imageV : this.imageBlueV, 18.0, 0.0);
				context.Restore();
			}
			return true;
		}

		public int HType { get; set; }

		public int VType { get; set; }

		private Xwt.Drawing.Image imageH;

		private Xwt.Drawing.Image imageV;

		private Xwt.Drawing.Image imageBlueH;

		private Xwt.Drawing.Image imageBlueV;
	}
}
