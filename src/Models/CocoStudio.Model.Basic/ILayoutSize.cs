using System;

namespace CocoStudio.Model
{
	public interface ILayoutSize
	{
		bool PercentWidthEnable { get; set; }

		bool PercentHeightEnable { get; set; }

		float PercentWidth { get; set; }

		float PercentHeight { get; set; }

		float SizeWidth { get; set; }

		float SizeHeight { get; set; }
	}
}
