using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(BlendFuncValue))]
	public class BlendFuncFrame : Frame
	{
		public BlendFuncFrame()
		{
			this.innerClass.SetTween(false);
		}

		public int Src
		{
			get
			{
				return (int)this._blendFuncValue.BlendSrc;
			}
			set
			{
				this._blendFuncValue = new BlendFuncValue((BlendSrc)value, this._blendFuncValue.BlendDst);
			}
		}

		public int Dst
		{
			get
			{
				return (int)this._blendFuncValue.BlendDst;
			}
			set
			{
				this._blendFuncValue = new BlendFuncValue(this._blendFuncValue.BlendSrc, (BlendDst)value);
			}
		}

		public BlendFuncValue Value
		{
			get
			{
				return this._blendFuncValue;
			}
			set
			{
				this._blendFuncValue = value;
			}
		}

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this._blendFuncValue = (BlendFuncValue)this.PropertyHandler.GetValue(node, null);
		}

		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this._blendFuncValue, null);
		}

		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			BlendFuncFrame blendFuncFrame = frame as BlendFuncFrame;
			if (blendFuncFrame != null)
			{
				blendFuncFrame.Value = this._blendFuncValue;
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

		private BlendFuncValue _blendFuncValue = new BlendFuncValue();
	}
}
