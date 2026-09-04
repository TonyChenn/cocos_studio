using System;
using System.Drawing;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D7 RID: 215
	[FrameExtension(typeof(Color))]
	public class ColorFrame : Frame
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001AB00 File Offset: 0x00018D00
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0001AB17 File Offset: 0x00018D17
		public virtual Color Color { get; set; }

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001AB20 File Offset: 0x00018D20
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Color = (Color)this.PropertyHandler.GetValue(node, null);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001AB64 File Offset: 0x00018D64
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			Color color = (this.nextFrame as ColorFrame).Color;
			this.betweenR = (int)(color.R - this.Color.R);
			this.betweenG = (int)(color.G - this.Color.G);
			this.betweenB = (int)(color.B - this.Color.B);
			if (!this.Tween)
			{
				this.PropertyHandler.SetValue(this.Node, this.Color, null);
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001AC08 File Offset: 0x00018E08
		protected override void OnApply(float percent, bool isChangeState)
		{
			byte red = (byte)((float)this.Color.R + (float)this.betweenR * percent);
			byte green = (byte)((float)this.Color.G + (float)this.betweenG * percent);
			byte blue = (byte)((float)this.Color.B + (float)this.betweenB * percent);
			Color color = Color.FromArgb((int)red, (int)green, (int)blue);
			this.PropertyHandler.SetValue(this.Node, color, null);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001AC94 File Offset: 0x00018E94
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			ColorFrame colorFrame = frame as ColorFrame;
			if (colorFrame != null)
			{
				colorFrame.Color = this.Color;
			}
		}

		// Token: 0x040002D8 RID: 728
		private int betweenR = 0;

		// Token: 0x040002D9 RID: 729
		private int betweenG = 0;

		// Token: 0x040002DA RID: 730
		private int betweenB = 0;
	}
}
