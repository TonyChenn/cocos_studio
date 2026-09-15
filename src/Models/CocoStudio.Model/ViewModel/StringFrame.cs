using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(string))]
	public class StringFrame : Frame
	{
		public virtual string Value { get; set; }

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (string)this.PropertyHandler.GetValue(node, null);
		}

		public StringFrame()
		{
			this.innerClass.SetTween(false);
		}

		public StringFrame(string value) : this()
		{
			this.Value = value;
		}

		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this.Value, null);
		}

		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			StringFrame stringFrame = frame as StringFrame;
			if (stringFrame != null)
			{
				stringFrame.Value = this.Value;
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
