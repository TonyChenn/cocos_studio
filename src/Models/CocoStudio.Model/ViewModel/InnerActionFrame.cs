using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(InnerActionValue))]
	public class InnerActionFrame : Frame
	{
		public string CurrentAniamtionName { get; set; }

		public InnerActionType InnerActionType { get; set; }

		public int SingleFrameIndex { get; set; }

		public InnerActionFrame()
		{
			this.innerClass.SetTween(false);
			this.forceApply = true;
		}

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
