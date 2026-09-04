using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D1 RID: 209
	[FrameExtension(typeof(bool))]
	public class BoolFrame : Frame
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0001A204 File Offset: 0x00018404
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x0001A21B File Offset: 0x0001841B
		public virtual bool Value { get; set; }

		// Token: 0x0600067C RID: 1660 RVA: 0x0001A224 File Offset: 0x00018424
		public BoolFrame()
		{
			this.innerClass.SetTween(false);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001A23C File Offset: 0x0001843C
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (bool)this.PropertyHandler.GetValue(node, null);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001A25D File Offset: 0x0001845D
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this.Value, null);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001A284 File Offset: 0x00018484
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			BoolFrame boolFrame = frame as BoolFrame;
			if (boolFrame != null)
			{
				boolFrame.Value = this.Value;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001A2B8 File Offset: 0x000184B8
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x0001A2CB File Offset: 0x000184CB
		public override bool Tween
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
