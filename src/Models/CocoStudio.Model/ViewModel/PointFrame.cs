using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(PointF))]
	public class PointFrame : Frame
	{
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

		public virtual float Y { get; set; }

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			PointF pointF = this.PropertyHandler.GetValue(node, null) as PointF;
			this.X = pointF.X;
			this.Y = pointF.Y;
		}

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

		internal override void UpdateValue(object delta)
		{
			if (delta != null)
			{
				PointF pointF = (PointF)delta;
				this.X += pointF.X;
				this.Y += pointF.Y;
			}
		}

		private PointF betweenPoint = new PointF();

		private float x;
	}
}
