using System;
using log4net;

namespace CocoStudio.Basic
{
	// Token: 0x02000008 RID: 8
	internal class CSLogger : ICSLog
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600001C RID: 28 RVA: 0x000023CC File Offset: 0x000005CC
		// (remove) Token: 0x0600001D RID: 29 RVA: 0x00002408 File Offset: 0x00000608
		public event Action<string> Output = delegate(string param0)
		{
		};

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002444 File Offset: 0x00000644
		private static ILog Log
		{
			get
			{
				return Log4Wrap.Logger;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000245E File Offset: 0x0000065E
		internal CSLogger(bool isOutput)
		{
			this.isOutput = true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002498 File Offset: 0x00000698
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

		// Token: 0x06000021 RID: 33 RVA: 0x000024D4 File Offset: 0x000006D4
		public void Debug(object message)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Debug(message);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002504 File Offset: 0x00000704
		public void Debug(object message, Exception exception)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Debug(message, exception);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002534 File Offset: 0x00000734
		public void Error(object message)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Error(message);
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002564 File Offset: 0x00000764
		public void Error(object message, Exception exception)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Error(message, exception);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002594 File Offset: 0x00000794
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

		// Token: 0x06000026 RID: 38 RVA: 0x000025CC File Offset: 0x000007CC
		public void Info(object message, Exception exception)
		{
			if (LogConfig.IsLogActivated)
			{
				this.RaiseOutput(message);
				CSLogger.Log.Info(message, exception);
			}
		}

		// Token: 0x0400001F RID: 31
		private bool isOutput;
	}
}
