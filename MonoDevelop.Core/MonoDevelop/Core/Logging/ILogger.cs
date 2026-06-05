using System;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x02000053 RID: 83
	public interface ILogger
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600028A RID: 650
		EnabledLoggingLevel EnabledLevel { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600028B RID: 651
		string Name { get; }

		// Token: 0x0600028C RID: 652
		void Log(LogLevel level, string message);
	}
}
