using System;
using System.IO;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000056 RID: 86
	public class FixZoomDrawArea : DrawingArea
	{
		// Token: 0x06000303 RID: 771 RVA: 0x0000C31C File Offset: 0x0000A51C
		public FixZoomDrawArea()
		{
			base.WidgetFlags |= (WidgetFlags.NoWindow | WidgetFlags.AppPaintable);
			this.imageH = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.position_arrow.png");
			this.imageV = this.RotationImage(this.imageH, PixbufRotation.Counterclockwise);
			this.imageBlueH = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.position_arrow_over.png");
			this.imageBlueV = this.RotationImage(this.imageBlueH, PixbufRotation.Counterclockwise);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000C390 File Offset: 0x0000A590
		private Xwt.Drawing.Image RotationImage(Xwt.Drawing.Image image, PixbufRotation rotation)
		{
			return Xwt.Drawing.Image.FromStream(new MemoryStream(image.GetPixbuf().RotateSimple(rotation).SaveToBuffer("png")));
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
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

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000C488 File Offset: 0x0000A688
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000C49F File Offset: 0x0000A69F
		public int HType { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000C4BF File Offset: 0x0000A6BF
		public int VType { get; set; }

		// Token: 0x04000164 RID: 356
		private Xwt.Drawing.Image imageH;

		// Token: 0x04000165 RID: 357
		private Xwt.Drawing.Image imageV;

		// Token: 0x04000166 RID: 358
		private Xwt.Drawing.Image imageBlueH;

		// Token: 0x04000167 RID: 359
		private Xwt.Drawing.Image imageBlueV;
	}
}
