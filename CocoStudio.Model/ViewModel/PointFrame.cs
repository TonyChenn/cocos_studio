using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D4 RID: 212
	[FrameExtension(typeof(PointF))]
	public class PointFrame : Frame
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0001A574 File Offset: 0x00018774
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x0001A58C File Offset: 0x0001878C
		public virtual float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0001A598 File Offset: 0x00018798
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x0001A5AF File Offset: 0x000187AF
		public virtual float Y { get; set; }

		// Token: 0x06000697 RID: 1687 RVA: 0x0001A5D0 File Offset: 0x000187D0
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			PointF pointF = this.PropertyHandler.GetValue(node, null) as PointF;
			this.X = pointF.X;
			this.Y = pointF.Y;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001A610 File Offset: 0x00018810
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			if (!(this.Node.Parent is ListViewObject) && !(this.Node.Parent is PageViewObject))
			{
				this.betweenPoint.X = (this.nextFrame as PointFrame).X - this.X;
				this.betweenPoint.Y = (this.nextFrame as PointFrame).Y - this.Y;
				if (!this.Tween)
				{
					PointF arg = new PointF(this.X, this.Y);
					this.PropertyHandler.SetValue(this.Node, arg, null);
				}
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001A6D0 File Offset: 0x000188D0
		protected override void OnApply(float percent, bool isChangeState)
		{
			if (!(this.Node.Parent is ListViewObject) && !(this.Node.Parent is PageViewObject))
			{
				PointF pointF = new PointF();
				pointF.X = this.X + this.betweenPoint.X * percent;
				pointF.Y = this.Y + this.betweenPoint.Y * percent;
				this.PropertyHandler.SetValue(this.Node, pointF, null);
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001A768 File Offset: 0x00018968
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			PointFrame pointFrame = frame as PointFrame;
			if (pointFrame != null)
			{
				pointFrame.X = this.X;
				pointFrame.Y = this.Y;
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001A7AC File Offset: 0x000189AC
		internal override void UpdateValue(object delta)
		{
			if (delta != null)
			{
				PointF pointF = (PointF)delta;
				this.X += pointF.X;
				this.Y += pointF.Y;
			}
		}

		// Token: 0x040002D1 RID: 721
		private PointF betweenPoint = new PointF();

		// Token: 0x040002D2 RID: 722
		private float x;
	}
}
