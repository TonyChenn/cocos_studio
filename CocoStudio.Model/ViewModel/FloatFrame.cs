using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D2 RID: 210
	[FrameExtension(typeof(float))]
	public class FloatFrame : Frame
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001A2D0 File Offset: 0x000184D0
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x0001A2E8 File Offset: 0x000184E8
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

		// Token: 0x06000685 RID: 1669 RVA: 0x0001A308 File Offset: 0x00018508
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.betweenValue = (this.nextFrame as FloatFrame).Value - this.Value;
			if (!this.Tween)
			{
				this.PropertyHandler.SetValue(this.Node, this.Value, null);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001A364 File Offset: 0x00018564
		protected override void OnApply(float percent, bool isChangeState)
		{
			float num = this.Value + this.betweenValue * percent;
			this.PropertyHandler.SetValue(this.Node, num, null);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001A3A1 File Offset: 0x000185A1
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (float)this.PropertyHandler.GetValue(node, null);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001A3C4 File Offset: 0x000185C4
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			FloatFrame floatFrame = frame as FloatFrame;
			if (floatFrame != null)
			{
				floatFrame.Value = this.Value;
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001A3F8 File Offset: 0x000185F8
		internal override void UpdateValue(object deltaValue)
		{
			if (deltaValue != null)
			{
				this.Value += (float)deltaValue;
			}
		}

		// Token: 0x040002CD RID: 717
		private float betweenValue = 0f;

		// Token: 0x040002CE RID: 718
		private float value;
	}
}
