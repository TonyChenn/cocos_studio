using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D5 RID: 213
	[FrameExtension(typeof(ScaleValue))]
	public class ScaleValueFrame : Frame
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0001A7F4 File Offset: 0x000189F4
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x0001A80B File Offset: 0x00018A0B
		public virtual float X { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x0001A814 File Offset: 0x00018A14
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x0001A82B File Offset: 0x00018A2B
		public virtual float Y { get; set; }

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001A84C File Offset: 0x00018A4C
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			ScaleValue scaleValue = this.PropertyHandler.GetValue(node, null) as ScaleValue;
			this.X = scaleValue.ScaleX;
			this.Y = scaleValue.ScaleY;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001A88C File Offset: 0x00018A8C
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.betweenValue.ScaleX = (this.nextFrame as ScaleValueFrame).X - this.X;
			this.betweenValue.ScaleY = (this.nextFrame as ScaleValueFrame).Y - this.Y;
			if (!this.Tween)
			{
				ScaleValue arg = new ScaleValue(this.X, this.Y, 0.1, -99999999.0, 99999999.0);
				this.PropertyHandler.SetValue(this.Node, arg, null);
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001A934 File Offset: 0x00018B34
		protected override void OnApply(float percent, bool isChangeState)
		{
			ScaleValue scaleValue = new ScaleValue();
			scaleValue.ScaleX = this.X + this.betweenValue.ScaleX * percent;
			scaleValue.ScaleY = this.Y + this.betweenValue.ScaleY * percent;
			this.PropertyHandler.SetValue(this.Node, scaleValue, null);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001A998 File Offset: 0x00018B98
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			ScaleValueFrame scaleValueFrame = frame as ScaleValueFrame;
			if (scaleValueFrame != null)
			{
				scaleValueFrame.X = this.X;
				scaleValueFrame.Y = this.Y;
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001A9DC File Offset: 0x00018BDC
		internal override void UpdateValue(object delta)
		{
			if (delta != null)
			{
				ScaleValue scaleValue = (ScaleValue)delta;
				this.X += scaleValue.ScaleX;
				this.Y += scaleValue.ScaleY;
			}
		}

		// Token: 0x040002D4 RID: 724
		private ScaleValue betweenValue = new ScaleValue();
	}
}
