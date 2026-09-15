using System;
using System.Drawing;

namespace CocoStudio.Model
{
	public interface ILabelEffect
	{
		bool ShadowEnabled { get; set; }

		float ShadowOffsetX { get; set; }

		float ShadowOffsetY { get; set; }

		int ShadowBlurRadius { get; set; }

		Color ShadowColor { get; set; }

		bool OutlineEnabled { get; set; }

		Color OutlineColor { get; set; }

		int OutlineSize { get; set; }
	}
}
