using System;

namespace OpenDialogs
{
	// Token: 0x02000048 RID: 72
	public struct WINDOWPOS
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00008124 File Offset: 0x00006324
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.x,
				":",
				this.y,
				":",
				this.cx,
				":",
				this.cy,
				":",
				((SWP_Flags)this.flags).ToString()
			});
		}

		// Token: 0x040002C2 RID: 706
		public IntPtr hwnd;

		// Token: 0x040002C3 RID: 707
		public IntPtr hwndAfter;

		// Token: 0x040002C4 RID: 708
		public int x;

		// Token: 0x040002C5 RID: 709
		public int y;

		// Token: 0x040002C6 RID: 710
		public int cx;

		// Token: 0x040002C7 RID: 711
		public int cy;

		// Token: 0x040002C8 RID: 712
		public uint flags;
	}
}
