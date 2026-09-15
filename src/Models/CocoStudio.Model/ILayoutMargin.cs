using System;

namespace CocoStudio.Model
{
	public interface ILayoutMargin
	{
		HorizontalBerthEdge HorizontalEdge { get; set; }

		VerticalBerthEdge VerticalEdge { get; set; }

		bool PercentHorizontalEnable { get; set; }

		bool PercentVertialEnable { get; set; }

		float PercentHorizontalMargin { get; set; }

		float PercentVerticalMargin { get; set; }

		float HorizontalMargin { get; set; }

		float VerticalMargin { get; set; }
	}
}
