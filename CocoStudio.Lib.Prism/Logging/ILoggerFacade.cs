using System;

namespace CocoStudio.Lib.Prism.Logging
{
	// Token: 0x02000003 RID: 3
	public interface ILoggerFacade
	{
		// Token: 0x06000001 RID: 1
		void Log(string message, Category category, Priority priority);
	}
}
