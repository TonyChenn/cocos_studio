using System;

namespace OpenDialogs
{
	// Token: 0x02000045 RID: 69
	public struct WINDOWINFO
	{
		// Token: 0x040002B2 RID: 690
		public uint cbSize;

		// Token: 0x040002B3 RID: 691
		public RECT rcWindow;

		// Token: 0x040002B4 RID: 692
		public RECT rcClient;

		// Token: 0x040002B5 RID: 693
		public uint dwStyle;

		// Token: 0x040002B6 RID: 694
		public uint dwExStyle;

		// Token: 0x040002B7 RID: 695
		public uint dwWindowStatus;

		// Token: 0x040002B8 RID: 696
		public uint cxWindowBorders;

		// Token: 0x040002B9 RID: 697
		public uint cyWindowBorders;

		// Token: 0x040002BA RID: 698
		public ushort atomWindowType;

		// Token: 0x040002BB RID: 699
		public ushort wCreatorVersion;
	}
}
