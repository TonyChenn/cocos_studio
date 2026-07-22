using System;
using System.Collections.Generic;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D0 RID: 208
	public class FrameComparer : IComparer<Frame>
	{
		// Token: 0x06000678 RID: 1656 RVA: 0x0001A1DC File Offset: 0x000183DC
		public int Compare(Frame x, Frame y)
		{
			return x.FrameIndex - y.FrameIndex;
		}
	}
}
