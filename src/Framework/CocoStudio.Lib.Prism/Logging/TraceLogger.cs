using System;

namespace CocoStudio.Lib.Prism.Logging
{
	public class TraceLogger : ILoggerFacade
	{
		public void Log(string message, Category category, Priority priority)
		{
			if (category == Category.Exception)
			{
			}
		}
	}
}
