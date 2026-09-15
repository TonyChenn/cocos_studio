using System;
using System.Drawing;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	public class Scale9DrawingArea : DrawingArea
	{
		public double TopMargin { get; set; }

		public double BottomMargin { get; set; }

		public double LeftMargin { get; set; }

		public double RightMargin { get; set; }

		public CurrentRange CurrentSelect { get; set; }

		public bool IsPress { get; set; }

		public bool CanDraw { get; set; }

		public Scale9DrawingArea()
		{
			this.image = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.scale9.png");
			this.CanDraw = true;
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (Cairo.Context context = CairoHelper.Create(evnt.Window))
			{
				context.DrawImage(this, this.image, 1.5, 1.5);
				context.SetSourceRGBA((double)this.defaultColor.R / 255.0, (double)this.defaultColor.G / 255.0, (double)this.defaultColor.B / 255.0, 1.0);
				context.Antialias = Antialias.None;
				context.LineWidth = 1.0;
				int num = 2;
				int num2 = num;
				while ((double)num2 <= 122.0)
				{
					context.MoveTo((double)num2, 2.0);
					context.LineTo((double)(num2 + 8), 2.0);
					num2 += 10;
				}
				num2 = num;
				while ((double)num2 <= 122.0)
				{
					context.MoveTo(2.0, (double)num2);
					context.LineTo(2.0, (double)(num2 + 8));
					num2 += 10;
				}
				num2 = num;
				while ((double)num2 <= 122.0)
				{
					context.MoveTo((double)num2, 102.0);
					context.LineTo((double)(num2 + 8), 102.0);
					num2 += 10;
				}
				num2 = num;
				while ((double)num2 <= 122.0)
				{
					context.MoveTo(102.0, (double)num2);
					context.LineTo(102.0, (double)(num2 + 8));
					num2 += 10;
				}
				context.Stroke();
				if (!this.CanDraw)
				{
					context.Save();
					context.Restore();
					return true;
				}
				if (this.CurrentSelect == CurrentRange.Left)
				{
					this.SetInnerLine(context, this.RightRange, 2.0, this.RightRange, 122.0, this.CurrentSelect, CurrentRange.Right);
					this.SetInnerLine(context, this.LeftRange, 2.0, this.LeftRange, 122.0, this.CurrentSelect, CurrentRange.Left);
				}
				else
				{
					this.SetInnerLine(context, this.LeftRange, 2.0, this.LeftRange, 122.0, this.CurrentSelect, CurrentRange.Left);
					this.SetInnerLine(context, this.RightRange, 2.0, this.RightRange, 122.0, this.CurrentSelect, CurrentRange.Right);
				}
				if (this.CurrentSelect == CurrentRange.Bottom)
				{
					this.SetInnerLine(context, 2.0, this.TopRange, 122.0, this.TopRange, this.CurrentSelect, CurrentRange.Top);
					this.SetInnerLine(context, 2.0, this.BottomRange, 122.0, this.BottomRange, this.CurrentSelect, CurrentRange.Bottom);
				}
				else
				{
					this.SetInnerLine(context, 2.0, this.BottomRange, 122.0, this.BottomRange, this.CurrentSelect, CurrentRange.Bottom);
					this.SetInnerLine(context, 2.0, this.TopRange, 122.0, this.TopRange, this.CurrentSelect, CurrentRange.Top);
				}
				if (this.TopRange >= 12.0)
				{
					context.MoveTo(132.0, 2.0);
					context.LineTo(132.0, this.TopRange);
					context.MoveTo(132.0, 2.0);
					context.LineTo(128.0, 6.0);
					context.MoveTo(132.0, 2.0);
					context.LineTo(136.0, 6.0);
					context.MoveTo(132.0, this.TopRange);
					context.LineTo(128.0, this.TopRange - 4.0);
					context.MoveTo(132.0, this.TopRange);
					context.LineTo(136.0, this.TopRange - 4.0);
				}
				if (this.BottomRange <= 92.0)
				{
					context.MoveTo(132.0, 102.0);
					context.LineTo(132.0, this.BottomRange);
					context.MoveTo(132.0, 102.0);
					context.LineTo(128.0, 98.0);
					context.MoveTo(132.0, 102.0);
					context.LineTo(136.0, 98.0);
					context.MoveTo(132.0, this.BottomRange);
					context.LineTo(128.0, this.BottomRange + 4.0);
					context.MoveTo(132.0, this.BottomRange);
					context.LineTo(136.0, this.BottomRange + 4.0);
				}
				if (this.LeftRange >= 12.0)
				{
					context.MoveTo(2.0, 132.0);
					context.LineTo(this.LeftRange, 132.0);
					context.MoveTo(2.0, 132.0);
					context.LineTo(6.0, 128.0);
					context.MoveTo(2.0, 132.0);
					context.LineTo(6.0, 136.0);
					context.MoveTo(this.LeftRange, 132.0);
					context.LineTo(this.LeftRange - 4.0, 128.0);
					context.MoveTo(this.LeftRange, 132.0);
					context.LineTo(this.LeftRange - 4.0, 136.0);
				}
				if (this.RightRange <= 92.0)
				{
					context.MoveTo(102.0, 132.0);
					context.LineTo(this.RightRange, 132.0);
					context.MoveTo(this.RightRange, 132.0);
					context.LineTo(this.RightRange + 4.0, 128.0);
					context.MoveTo(this.RightRange, 132.0);
					context.LineTo(this.RightRange + 4.0, 136.0);
					context.MoveTo(102.0, 132.0);
					context.LineTo(98.0, 128.0);
					context.MoveTo(102.0, 132.0);
					context.LineTo(98.0, 136.0);
				}
				context.Stroke();
				context.Save();
				context.Restore();
			}
			return true;
		}

		private void SetInnerLine(Cairo.Context cr, double movex, double moveY, double lineX, double lineY, CurrentRange range, CurrentRange selectRange)
		{
			if (range == selectRange && this.IsPress)
			{
				cr.SetSourceRGBA((double)this.selelctColor.R / 255.0, (double)this.selelctColor.G / 255.0, (double)this.selelctColor.B / 255.0, 1.0);
				cr.MoveTo(movex, moveY);
				cr.LineTo(lineX, lineY);
				cr.Stroke();
				cr.SetSourceRGBA((double)this.defaultColor.R / 255.0, (double)this.defaultColor.G / 255.0, (double)this.defaultColor.B / 255.0, 1.0);
			}
			else
			{
				cr.MoveTo(movex, moveY);
				cr.LineTo(lineX, lineY);
				cr.Stroke();
			}
		}

		public double LeftRange
		{
			get
			{
				return this.leftRange;
			}
			set
			{
				this.leftRange = value;
			}
		}

		public double RightRange
		{
			get
			{
				return this.rightRange;
			}
			set
			{
				this.rightRange = value;
			}
		}

		public double TopRange
		{
			get
			{
				return this.topRange;
			}
			set
			{
				this.topRange = value;
			}
		}

		public double BottomRange
		{
			get
			{
				return this.bottomRange;
			}
			set
			{
				this.bottomRange = value;
			}
		}

		private const double innerMax = 122.0;

		private const double innerMin = 22.0;

		private const double arrowMax = 102.0;

		private const double outMax = 132.0;

		private const double outMin = 2.0;

		private const double minValue = 12.0;

		private const double maxValue = 92.0;

		private Xwt.Drawing.Image image;

		private System.Drawing.Color defaultColor = System.Drawing.Color.FromArgb(255, 171, 174, 183);

		private System.Drawing.Color selelctColor = System.Drawing.Color.FromArgb(255, 8, 114, 245);

		private double leftRange = 22.0;

		private double rightRange = 82.0;

		private double topRange = 22.0;

		private double bottomRange = 82.0;
	}
}
