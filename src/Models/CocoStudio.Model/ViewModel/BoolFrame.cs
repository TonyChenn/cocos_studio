using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(bool))]
	public class BoolFrame : Frame
	{
		public virtual bool Value { get; set; }

		public BoolFrame()
		{
			this.innerClass.SetTween(false);
		}

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (bool)this.PropertyHandler.GetValue(node, null);
		}

		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this.Value, null);
		}

		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			BoolFrame boolFrame = frame as BoolFrame;
			if (boolFrame != null)
			{
				boolFrame.Value = this.Value;
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
