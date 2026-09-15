using System;
using System.Drawing;
using Cairo;
using Gdk;
using Gtk;

namespace CocoStudio.Model.Editor
{
	public class FixZoomRectangle : DrawingArea
	{
		public bool IsTop { get; set; }

		public bool IsBottom { get; set; }

		public bool IsLeft { get; set; }

		public bool IsRight { get; set; }

		public bool IsWidthCenter { get; set; }

		public bool IsHeightCenter { get; set; }

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

		private const int begin = 5;

		private const int rectSize = 12;

		private const int rectBound = 10;

		private const int rectBegin = 8;

		private const int stepBound = 4;

		private const int splitBound = 6;

		private const int rectHeight = 75;

		private const int rectWidth = 150;

		private bool status = true;

		private double currentWidth = 75.0;

		private double currentHeight = 37.0;
	}
}
