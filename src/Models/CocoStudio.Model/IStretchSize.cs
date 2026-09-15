using System;

namespace CocoStudio.Model
{
	public interface IStretchSize
	{
		bool StretchWidthEnable { get; set; }

		bool StretchHeightEnable { get; set; }

		bool CanShowStretch { get; set; }
	}
}
