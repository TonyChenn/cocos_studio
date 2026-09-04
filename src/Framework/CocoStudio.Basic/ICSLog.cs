using System;

namespace CocoStudio.Basic
{
	// Token: 0x02000005 RID: 5
	public interface ICSLog
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000001 RID: 1
		// (remove) Token: 0x06000002 RID: 2
		event Action<string> Output;

		// Token: 0x06000003 RID: 3
		void Debug(object message);

		// Token: 0x06000004 RID: 4
		void Debug(object message, Exception exception);

		// Token: 0x06000005 RID: 5
		void Error(object message);

		// Token: 0x06000006 RID: 6
		void Error(object message, Exception exception);

		// Token: 0x06000007 RID: 7
		void Info(object message, bool log = true);

		// Token: 0x06000008 RID: 8
		void Info(object message, Exception exception);
	}
}
