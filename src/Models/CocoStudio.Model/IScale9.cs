using System;

namespace CocoStudio.Model
{
	public interface IScale9
	{
		bool Scale9Enable { get; set; }

		int LeftEage { get; set; }

		int RightEage { get; set; }

		int TopEage { get; set; }

		int BottomEage { get; set; }

		SizeF ResourceSize { get; }
	}
}
