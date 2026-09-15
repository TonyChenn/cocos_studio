using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(int))]
	public class IntFrame : Frame
	{
		public virtual int Value
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
			this.betweenValue = (this.nextFrame as IntFrame).Value - this.Value;
			if (!this.Tween)
			{
				this.PropertyHandler.SetValue(this.Node, this.Value, null);
			}
		}

		protected override void OnApply(float percent, bool isChangeState)
		{
			float num = (float)this.Value + (float)this.betweenValue * percent;
			this.PropertyHandler.SetValue(this.Node, (int)num, null);
		}

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (int)this.PropertyHandler.GetValue(node, null);
		}

		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			IntFrame intFrame = frame as IntFrame;
			if (intFrame != null)
			{
				intFrame.Value = this.Value;
			}
		}

		internal override void UpdateValue(object delta)
		{
			if (delta != null)
			{
				this.Value += (int)delta;
			}
		}

		private int betweenValue = 0;

		private int value;
	}
}
