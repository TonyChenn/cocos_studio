using System;
using System.Drawing;
using Cairo;
using Gdk;
using Gtk;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000055 RID: 85
	public class FixZoomRectangle : DrawingArea
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000BDA0 File Offset: 0x00009FA0
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000BDB7 File Offset: 0x00009FB7
		public bool IsTop { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000BDC0 File Offset: 0x00009FC0
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000BDD7 File Offset: 0x00009FD7
		public bool IsBottom { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0000BDF7 File Offset: 0x00009FF7
		public bool IsLeft { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000BE00 File Offset: 0x0000A000
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000BE17 File Offset: 0x0000A017
		public bool IsRight { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000BE20 File Offset: 0x0000A020
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000BE37 File Offset: 0x0000A037
		public bool IsWidthCenter { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000BE40 File Offset: 0x0000A040
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0000BE57 File Offset: 0x0000A057
		public bool IsHeightCenter { get; set; }

		// Token: 0x06000300 RID: 768 RVA: 0x0000BE60 File Offset: 0x0000A060
		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				System.Drawing.Color color = System.Drawing.Color.FromArgb(255, 40, 40, 41);
				context.SetSourceRGBA((double)color.R / 255.0, (double)color.G / 255.0, (double)color.B / 255.0, 1.0);
				context.Rectangle(0.0, 0.0, 150.0, 75.0);
				context.Fill();
				context.Stroke();
				System.Drawing.Color color2 = System.Drawing.Color.FromArgb(255, 126, 124, 135);
				context.SetSourceRGBA((double)color2.R / 255.0, (double)color2.G / 255.0, (double)color2.B / 255.0, 1.0);
				context.Rectangle(5.0, 5.0, this.currentWidth - 5.0, this.currentHeight - 5.0);
				context.Fill();
				context.Stroke();
				System.Drawing.Color color3 = System.Drawing.Color.FromArgb(255, 254, 254, 250);
				context.SetSourceRGBA((double)color3.R / 255.0, (double)color3.G / 255.0, (double)color3.B / 255.0, 1.0);
				int num = (int)this.currentWidth - 5;
				int num2 = (int)this.currentHeight - 5;
				int num3 = num / 2 - 3;
				int num4 = num2 / 2 - 3;
				int num5;
				int num6;
				if (!this.IsLeft && this.IsRight)
				{
					if (this.IsWidthCenter)
					{
						num5 = num / 2;
						num6 = num3;
					}
					else
					{
						num5 = num - 12;
						num6 = 12;
					}
				}
				else if (this.IsLeft && this.IsRight)
				{
					if (this.IsWidthCenter)
					{
						num5 = 8;
						num6 = num - 6;
					}
					else
					{
						num5 = num / 2;
						num6 = 12;
					}
				}
				else
				{
					num5 = 8;
					if (this.IsWidthCenter)
					{
						num6 = num3;
					}
					else
					{
						num6 = 12;
					}
				}
				int num7;
				int num8;
				if ((!this.IsTop && this.IsBottom) || (!this.IsTop && !this.IsBottom))
				{
					if (this.IsHeightCenter)
					{
						num7 = num2 / 2;
						num8 = num4;
					}
					else
					{
						num7 = num2 - 12;
						num8 = 12;
					}
				}
				else if (this.IsTop && this.IsBottom)
				{
					if (this.IsHeightCenter)
					{
						num7 = 8;
						num8 = num2 - 6;
					}
					else
					{
						num7 = num2 / 2;
						num8 = 12;
					}
				}
				else
				{
					num7 = 8;
					if (this.IsHeightCenter)
					{
						num8 = num4;
					}
					else
					{
						num8 = 12;
					}
				}
				context.Rectangle((double)num5, (double)num7, (double)num6, (double)num8);
				context.Fill();
				context.Fill();
				context.Stroke();
				context.Restore();
			}
			return base.OnExposeEvent(evnt);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000C230 File Offset: 0x0000A430
		public double TimerTick()
		{
			if (this.status)
			{
				this.currentWidth += 4.0;
				this.currentHeight += 2.0;
			}
			else
			{
				this.currentWidth -= 4.0;
				this.currentHeight -= 2.0;
			}
			if (this.currentWidth >= 140.0)
			{
				this.status = false;
			}
			if (this.currentWidth <= 75.0)
			{
				this.status = true;
			}
			return this.currentWidth;
		}

		// Token: 0x04000153 RID: 339
		private const int begin = 5;

		// Token: 0x04000154 RID: 340
		private const int rectSize = 12;

		// Token: 0x04000155 RID: 341
		private const int rectBound = 10;

		// Token: 0x04000156 RID: 342
		private const int rectBegin = 8;

		// Token: 0x04000157 RID: 343
		private const int stepBound = 4;

		// Token: 0x04000158 RID: 344
		private const int splitBound = 6;

		// Token: 0x04000159 RID: 345
		private const int rectHeight = 75;

		// Token: 0x0400015A RID: 346
		private const int rectWidth = 150;

		// Token: 0x0400015B RID: 347
		private bool status = true;

		// Token: 0x0400015C RID: 348
		private double currentWidth = 75.0;

		// Token: 0x0400015D RID: 349
		private double currentHeight = 37.0;
	}
}
