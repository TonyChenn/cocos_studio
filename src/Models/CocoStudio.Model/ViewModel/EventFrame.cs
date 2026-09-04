using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D8 RID: 216
	public class EventFrame : StringFrame
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x0001ACC8 File Offset: 0x00018EC8
		public EventFrame()
		{
			this.forceApply = true;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001ACDC File Offset: 0x00018EDC
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			base.OnEnter(nextFrameIndex, isChangeState);
			if (TimelineActionManager.Instance.CurrentFrameIndex != this.FrameIndex)
			{
				this.PropertyHandler.SetValue(this.Node, "", null);
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001AD28 File Offset: 0x00018F28
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
