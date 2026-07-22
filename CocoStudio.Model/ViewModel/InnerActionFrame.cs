using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000E2 RID: 226
	[FrameExtension(typeof(InnerActionValue))]
	public class InnerActionFrame : Frame
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0001CEDB File Offset: 0x0001B0DB
		public string CurrentAniamtionName { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0001CEFB File Offset: 0x0001B0FB
		public InnerActionType InnerActionType { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001CF04 File Offset: 0x0001B104
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0001CF1B File Offset: 0x0001B11B
		public int SingleFrameIndex { get; set; }

		// Token: 0x06000723 RID: 1827 RVA: 0x0001CF24 File Offset: 0x0001B124
		public InnerActionFrame()
		{
			this.innerClass.SetTween(false);
			this.forceApply = true;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001CF44 File Offset: 0x0001B144
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			IInnerActoinNode innerActoinNode = this.Node as IInnerActoinNode;
			if (innerActoinNode != null)
			{
				InnerActionValue actionValue = innerActoinNode.ActionValue;
				this.CurrentAniamtionName = actionValue.ActivedAnimationName;
				this.InnerActionType = actionValue.ActionType;
				this.SingleFrameIndex = actionValue.SingleFrameIndex;
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001CF98 File Offset: 0x0001B198
		protected override void OnApply(float percent, bool isChangeState)
		{
			if (isChangeState)
			{
				IInnerActoinNode innerActoinNode = this.Node as IInnerActoinNode;
				if (innerActoinNode != null)
				{
					InnerActionValue actionValue = new InnerActionValue(this.InnerActionType, innerActoinNode.ActionValue.AnimationNames, this.CurrentAniamtionName, this.SingleFrameIndex);
					innerActoinNode.ApplyStep(actionValue, this.FrameIndex);
				}
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001CFF4 File Offset: 0x0001B1F4
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			if (isChangeState)
			{
				bool flag = string.IsNullOrEmpty(this.CurrentAniamtionName) && this.InnerActionType == InnerActionType.LoopAction;
				if (!flag)
				{
					IInnerActoinNode innerActoinNode = this.Node as IInnerActoinNode;
					if (innerActoinNode != null)
					{
						InnerActionValue actionValue = new InnerActionValue(this.InnerActionType, innerActoinNode.ActionValue.AnimationNames, this.CurrentAniamtionName, this.SingleFrameIndex);
						innerActoinNode.ApplyActionValue(actionValue);
					}
				}
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001D070 File Offset: 0x0001B270
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			InnerActionFrame innerActionFrame = frame as InnerActionFrame;
			if (innerActionFrame != null)
			{
				innerActionFrame.CurrentAniamtionName = this.CurrentAniamtionName;
				innerActionFrame.InnerActionType = this.InnerActionType;
				innerActionFrame.SingleFrameIndex = this.SingleFrameIndex;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001D0C0 File Offset: 0x0001B2C0
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001D0D3 File Offset: 0x0001B2D3
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
