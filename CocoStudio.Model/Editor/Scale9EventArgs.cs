using System;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200006B RID: 107
	public class Scale9EventArgs : EventArgs
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00011778 File Offset: 0x0000F978
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0001178F File Offset: 0x0000F98F
		public CurrentRange Type { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00011798 File Offset: 0x0000F998
		// (set) Token: 0x0600039F RID: 927 RVA: 0x000117AF File Offset: 0x0000F9AF
		public double Left { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x000117B8 File Offset: 0x0000F9B8
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x000117CF File Offset: 0x0000F9CF
		public double Right { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x000117D8 File Offset: 0x0000F9D8
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x000117EF File Offset: 0x0000F9EF
		public double Top { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000117F8 File Offset: 0x0000F9F8
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0001180F File Offset: 0x0000FA0F
		public double Bottom { get; set; }

		// Token: 0x060003A6 RID: 934 RVA: 0x00011818 File Offset: 0x0000FA18
		public Scale9EventArgs(CurrentRange type, double left, double right, double top, double bottom)
		{
			this.Type = type;
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
		}
	}
}
