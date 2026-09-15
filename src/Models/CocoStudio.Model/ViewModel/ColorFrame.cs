using System;
using System.Drawing;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(Color))]
	public class ColorFrame : Frame
	{
		public virtual Color Color { get; set; }

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Color = (Color)this.PropertyHandler.GetValue(node, null);
		}

		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			Color color = (this.nextFrame as ColorFrame).Color;
			this.betweenR = (int)(color.R - this.Color.R);
			this.betweenG = (int)(color.G - this.Color.G);
			this.betweenB = (int)(color.B - this.Color.B);
			if (!this.Tween)
			{
				this.PropertyHandler.SetValue(this.Node, this.Color, null);
			}
		}

		protected override void OnApply(float percent, bool isChangeState)
		{
			byte red = (byte)((float)this.Color.R + (float)this.betweenR * percent);
			byte green = (byte)((float)this.Color.G + (float)this.betweenG * percent);
			byte blue = (byte)((float)this.Color.B + (float)this.betweenB * percent);
			Color color = Color.FromArgb((int)red, (int)green, (int)blue);
			this.PropertyHandler.SetValue(this.Node, color, null);
		}

		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			ColorFrame colorFrame = frame as ColorFrame;
			if (colorFrame != null)
			{
				colorFrame.Color = this.Color;
			}
		}

		private int betweenR = 0;

		private int betweenG = 0;

		private int betweenB = 0;
	}
}
