using System;

namespace CocoStudio.Model
{
	public interface IFlipped
	{
		bool FlipX { get; set; }

		bool FlipY { get; set; }

		bool IsReverse { get; }
	}
}
