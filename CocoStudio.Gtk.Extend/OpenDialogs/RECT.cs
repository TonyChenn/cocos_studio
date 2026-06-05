using System;

namespace OpenDialogs
{
	// Token: 0x02000047 RID: 71
	public struct RECT
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007FD0 File Offset: 0x000061D0
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00007FF4 File Offset: 0x000061F4
		public POINT Location
		{
			get
			{
				return new POINT((int)this.left, (int)this.top);
			}
			set
			{
				this.right -= this.left - (uint)value.x;
				this.bottom -= this.bottom - (uint)value.y;
				this.left = (uint)value.x;
				this.top = (uint)value.y;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008054 File Offset: 0x00006254
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00008073 File Offset: 0x00006273
		public uint Width
		{
			get
			{
				return this.right - this.left;
			}
			set
			{
				this.right = this.left + value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00008084 File Offset: 0x00006284
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x000080A3 File Offset: 0x000062A3
		public uint Height
		{
			get
			{
				return this.bottom - this.top;
			}
			set
			{
				this.bottom = this.top + value;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000080B4 File Offset: 0x000062B4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.left,
				":",
				this.top,
				":",
				this.right,
				":",
				this.bottom
			});
		}

		// Token: 0x040002BE RID: 702
		public uint left;

		// Token: 0x040002BF RID: 703
		public uint top;

		// Token: 0x040002C0 RID: 704
		public uint right;

		// Token: 0x040002C1 RID: 705
		public uint bottom;
	}
}
