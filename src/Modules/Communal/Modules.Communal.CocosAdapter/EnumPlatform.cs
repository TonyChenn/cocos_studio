using System;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000024 RID: 36
	[Flags]
	public enum EnumPlatform
	{
		// Token: 0x0400006B RID: 107
		Unknown = -1,
		// Token: 0x0400006C RID: 108
		None = 0,
		// Token: 0x0400006D RID: 109
		Android = 1,
		// Token: 0x0400006E RID: 110
		iOS = 2,
		// Token: 0x0400006F RID: 111
		Web = 4,
		// Token: 0x04000070 RID: 112
		Windows = 8,
		// Token: 0x04000071 RID: 113
		Mac = 16,
		// Token: 0x04000072 RID: 114
		Simulator = 32
	}
}
