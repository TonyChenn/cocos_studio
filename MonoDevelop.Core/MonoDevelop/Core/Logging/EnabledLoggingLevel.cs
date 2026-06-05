using System;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x02000052 RID: 82
	[Flags]
	public enum EnabledLoggingLevel
	{
		// Token: 0x040000ED RID: 237
		Fatal = 1,
		// Token: 0x040000EE RID: 238
		Error = 2,
		// Token: 0x040000EF RID: 239
		Warn = 4,
		// Token: 0x040000F0 RID: 240
		Info = 8,
		// Token: 0x040000F1 RID: 241
		Debug = 16,
		// Token: 0x040000F2 RID: 242
		None = 0,
		// Token: 0x040000F3 RID: 243
		UpToFatal = 1,
		// Token: 0x040000F4 RID: 244
		UpToError = 3,
		// Token: 0x040000F5 RID: 245
		UpToWarn = 7,
		// Token: 0x040000F6 RID: 246
		UpToInfo = 15,
		// Token: 0x040000F7 RID: 247
		UpToDebug = 31,
		// Token: 0x040000F8 RID: 248
		All = 31
	}
}
