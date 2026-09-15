using System;

namespace CocoStudio.Model.ViewModel
{
	public class EventFrame : StringFrame
	{
		public EventFrame()
		{
			this.forceApply = true;
		}

		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			base.OnEnter(nextFrameIndex, isChangeState);
			if (TimelineActionManager.Instance.CurrentFrameIndex != this.FrameIndex)
			{
				this.PropertyHandler.SetValue(this.Node, "", null);
			}
		}

		protected override void OnApply(float percent, bool isChangeState)
		{
			base.OnApply(percent, isChangeState);
			if (TimelineActionManager.Instance.CurrentFrameIndex != this.FrameIndex)
			{
				this.PropertyHandler.SetValue(this.Node, "", null);
			}
			else
			{
				this.PropertyHandler.SetValue(this.Node, this.Value, null);
			}
		}
	}
}
