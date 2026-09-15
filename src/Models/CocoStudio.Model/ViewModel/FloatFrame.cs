using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(float))]
	public class FloatFrame : Frame
	{
		public virtual float Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.betweenValue = (this.nextFrame as FloatFrame).Value - this.Value;
			if (!this.Tween)
			{
				this.PropertyHandler.SetValue(this.Node, this.Value, null);
			}
		}

		protected override void OnApply(float percent, bool isChangeState)
		{
			float num = this.Value + this.betweenValue * percent;
			this.PropertyHandler.SetValue(this.Node, num, null);
		}

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (float)this.PropertyHandler.GetValue(node, null);
		}

		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			FloatFrame floatFrame = frame as FloatFrame;
			if (floatFrame != null)
			{
				floatFrame.Value = this.Value;
			}
		}

		internal override void UpdateValue(object deltaValue)
		{
			if (deltaValue != null)
			{
				this.Value += (float)deltaValue;
			}
		}

		private float betweenValue = 0f;

		private float value;
	}
}
