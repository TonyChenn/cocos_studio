using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D3 RID: 211
	[FrameExtension(typeof(int))]
	public class IntFrame : Frame
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001A424 File Offset: 0x00018624
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0001A43C File Offset: 0x0001863C
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

		// Token: 0x0600068D RID: 1677 RVA: 0x0001A458 File Offset: 0x00018658
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.betweenValue = (this.nextFrame as IntFrame).Value - this.Value;
			if (!this.Tween)
			{
				this.PropertyHandler.SetValue(this.Node, this.Value, null);
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001A4B4 File Offset: 0x000186B4
		protected override void OnApply(float percent, bool isChangeState)
		{
			float num = (float)this.Value + (float)this.betweenValue * percent;
			this.PropertyHandler.SetValue(this.Node, (int)num, null);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001A4F3 File Offset: 0x000186F3
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (int)this.PropertyHandler.GetValue(node, null);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001A514 File Offset: 0x00018714
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			IntFrame intFrame = frame as IntFrame;
			if (intFrame != null)
			{
				intFrame.Value = this.Value;
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001A548 File Offset: 0x00018748
		internal override void UpdateValue(object delta)
		{
			if (delta != null)
			{
				this.Value += (int)delta;
			}
		}

		// Token: 0x040002CF RID: 719
		private int betweenValue = 0;

		// Token: 0x040002D0 RID: 720
		private int value;
	}
}
