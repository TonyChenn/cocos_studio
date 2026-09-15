using System;
using log4net;

namespace CocoStudio.Basic
{
	internal class CSLogger : ICSLog
	{
		public event Action<string> Output = delegate(string param0)
		{
		};

		private static ILog Log
		{
			get
			{
				return Log4Wrap.Logger;
			}
		}

		internal CSLogger(bool isOutput)
		{
			this.isOutput = true;
		}

		private void RaiseOutput(object message)
		{
			if (message != null)
			{
				if (this.isOutput)
				{
					this.Output(message.ToString());
				}
			}
		}

		public void Debug(object message)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Debug(message);
			}
		}

		public void Debug(object message, Exception exception)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Debug(message, exception);
			}
		}

		public void Error(object message)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Error(message);
			}
		}

		public void Error(object message, Exception exception)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Error(message, exception);
			}
		}

		public void Info(object message, bool log)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				if (log)
				{
					CSLogger.Log.Info(message);
				}
			}
		}

		public void Info(object message, Exception exception)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Info(message, exception);
			}
		}

		private bool isOutput;
	}
}
