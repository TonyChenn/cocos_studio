using System;

namespace CocoStudio.Lib.Prism.Logging
{
	// Token: 0x02000007 RID: 7
	public class TraceLogger : ILoggerFacade
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002154 File Offset: 0x00000354
		public void Log(string message, Category category, Priority priority)
		{
			if (category == Category.Exception)
			{
			}
		}
	}
}
