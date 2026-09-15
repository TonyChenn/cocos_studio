using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(ScaleValue))]
	public class ScaleValueFrame : Frame
	{
		public virtual float X { get; set; }

		public virtual float Y { get; set; }

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			ScaleValue scaleValue = this.PropertyHandler.GetValue(node, null) as ScaleValue;
			this.X = scaleValue.ScaleX;
			this.Y = scaleValue.ScaleY;
		}

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

		protected override void OnApply(float percent, bool isChangeState)
		{
			ScaleValue scaleValue = new ScaleValue();
			scaleValue.ScaleX = this.X + this.betweenValue.ScaleX * percent;
			scaleValue.ScaleY = this.Y + this.betweenValue.ScaleY * percent;
			this.PropertyHandler.SetValue(this.Node, scaleValue, null);
		}

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

		internal override void UpdateValue(object delta)
		{
			if (delta != null)
			{
				ScaleValue scaleValue = (ScaleValue)delta;
				this.X += scaleValue.ScaleX;
				this.Y += scaleValue.ScaleY;
			}
		}

		private ScaleValue betweenValue = new ScaleValue();
	}
}
