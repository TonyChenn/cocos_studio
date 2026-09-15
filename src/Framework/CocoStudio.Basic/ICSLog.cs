using System;

namespace CocoStudio.Basic
{
	public interface ICSLog
	{
		event Action<string> Output;

		void Debug(object message);

		void Debug(object message, Exception exception);

		void Error(object message);

		void Error(object message, Exception exception);

		void Info(object message, bool log = true);

		void Info(object message, Exception exception);
	}
}
