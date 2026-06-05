using System;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000006 RID: 6
	public interface IUserStatistics
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8
		// (set) Token: 0x06000009 RID: 9
		EditorInfo EditorInfo { get; set; }

		// Token: 0x0600000A RID: 10
		void UnHandledException(Exception ex, string feedbackInfo);

		// Token: 0x0600000B RID: 11
		void Exit();

		// Token: 0x0600000C RID: 12
		void ExitAll();
	}
}
