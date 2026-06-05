using System;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000010 RID: 16
	internal enum UpdateWindowStatus
	{
		// Token: 0x04000031 RID: 49
		Init,
		// Token: 0x04000032 RID: 50
		Downloading,
		// Token: 0x04000033 RID: 51
		Finished,
		// Token: 0x04000034 RID: 52
		Failed,
		// Token: 0x04000035 RID: 53
		LowVersion
	}
}
