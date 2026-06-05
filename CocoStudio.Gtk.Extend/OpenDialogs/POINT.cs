using System;
using System.Drawing;

namespace OpenDialogs
{
	// Token: 0x02000046 RID: 70
	public struct POINT
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00007FA0 File Offset: 0x000061A0
		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007FB1 File Offset: 0x000061B1
		public POINT(Point point)
		{
			this.x = point.X;
			this.y = point.Y;
		}

		// Token: 0x040002BC RID: 700
		public int x;

		// Token: 0x040002BD RID: 701
		public int y;
	}
}
