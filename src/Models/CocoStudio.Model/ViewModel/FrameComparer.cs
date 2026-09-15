using System;
using System.Collections.Generic;

namespace CocoStudio.Model.ViewModel
{
	public class FrameComparer : IComparer<Frame>
	{
		public int Compare(Frame x, Frame y)
		{
			return x.FrameIndex - y.FrameIndex;
		}
	}
}
