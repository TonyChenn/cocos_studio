using System;

namespace OpenDialogs
{
	public struct WINDOWPOS
	{
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

		public IntPtr hwnd;

		public IntPtr hwndAfter;

		public int x;

		public int y;

		public int cx;

		public int cy;

		public uint flags;
	}
}
