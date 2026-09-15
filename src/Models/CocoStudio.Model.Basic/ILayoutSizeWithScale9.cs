using System;

namespace CocoStudio.Model
{
	public interface ILayoutSizeWithScale9 : ILayoutSize
	{
		bool Scale9Enable { get; set; }

		int LeftEage { get; set; }

		int RightEage { get; set; }

		int TopEage { get; set; }

		int BottomEage { get; set; }
	}
}
