using System;
using System.Drawing;
using Cairo;
using Gdk;
using Gtk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000069 RID: 105
	public class Scale9DrawingArea : DrawingArea
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600037B RID: 891 RVA: 0x000106B8 File Offset: 0x0000E8B8
		// (set) Token: 0x0600037C RID: 892 RVA: 0x000106CF File Offset: 0x0000E8CF
		public double TopMargin { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600037D RID: 893 RVA: 0x000106D8 File Offset: 0x0000E8D8
		// (set) Token: 0x0600037E RID: 894 RVA: 0x000106EF File Offset: 0x0000E8EF
		public double BottomMargin { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600037F RID: 895 RVA: 0x000106F8 File Offset: 0x0000E8F8
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0001070F File Offset: 0x0000E90F
		public double LeftMargin { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00010718 File Offset: 0x0000E918
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0001072F File Offset: 0x0000E92F
		public double RightMargin { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00010738 File Offset: 0x0000E938
		// (set) Token: 0x06000384 RID: 900 RVA: 0x0001074F File Offset: 0x0000E94F
		public CurrentRange CurrentSelect { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00010758 File Offset: 0x0000E958
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0001076F File Offset: 0x0000E96F
		public bool IsPress { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00010778 File Offset: 0x0000E978
		// (set) Token: 0x06000388 RID: 904 RVA: 0x0001078F File Offset: 0x0000E98F
		public bool CanDraw { get; set; }

		// Token: 0x06000389 RID: 905 RVA: 0x00010798 File Offset: 0x0000E998
		public Scale9DrawingArea()
		{
			this.image = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.scale9.png");
			this.CanDraw = true;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001083C File Offset: 0x0000EA3C
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

		// Token: 0x0600038B RID: 907 RVA: 0x00011080 File Offset: 0x0000F280
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

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00011188 File Offset: 0x0000F388
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000111A0 File Offset: 0x0000F3A0
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000111AC File Offset: 0x0000F3AC
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000111C4 File Offset: 0x0000F3C4
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000111D0 File Offset: 0x0000F3D0
		// (set) Token: 0x06000391 RID: 913 RVA: 0x000111E8 File Offset: 0x0000F3E8
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000392 RID: 914 RVA: 0x000111F4 File Offset: 0x0000F3F4
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0001120C File Offset: 0x0000F40C
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

		// Token: 0x040001BF RID: 447
		private const double innerMax = 122.0;

		// Token: 0x040001C0 RID: 448
		private const double innerMin = 22.0;

		// Token: 0x040001C1 RID: 449
		private const double arrowMax = 102.0;

		// Token: 0x040001C2 RID: 450
		private const double outMax = 132.0;

		// Token: 0x040001C3 RID: 451
		private const double outMin = 2.0;

		// Token: 0x040001C4 RID: 452
		private const double minValue = 12.0;

		// Token: 0x040001C5 RID: 453
		private const double maxValue = 92.0;

		// Token: 0x040001C6 RID: 454
		private Xwt.Drawing.Image image;

		// Token: 0x040001C7 RID: 455
		private System.Drawing.Color defaultColor = System.Drawing.Color.FromArgb(255, 171, 174, 183);

		// Token: 0x040001C8 RID: 456
		private System.Drawing.Color selelctColor = System.Drawing.Color.FromArgb(255, 8, 114, 245);

		// Token: 0x040001C9 RID: 457
		private double leftRange = 22.0;

		// Token: 0x040001CA RID: 458
		private double rightRange = 82.0;

		// Token: 0x040001CB RID: 459
		private double topRange = 22.0;

		// Token: 0x040001CC RID: 460
		private double bottomRange = 82.0;
	}
}
