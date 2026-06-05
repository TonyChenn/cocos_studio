using System;
using MonoDevelop.Core.Instrumentation;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x020000F9 RID: 249
	internal class InstrumentationLogger : ILogger
	{
		// Token: 0x060008C6 RID: 2246 RVA: 0x00022EA4 File Offset: 0x000210A4
		public void Log(LogLevel level, string message)
		{
			if (!InstrumentationService.IsLoggingMessage)
			{
				switch (level)
				{
				case LogLevel.Fatal:
					Counters.LogFatalErrors.Inc(message);
					return;
				case LogLevel.Error:
					Counters.LogErrors.Inc(message);
					return;
				case (LogLevel)3:
					break;
				case LogLevel.Warn:
					Counters.LogWarnings.Inc(message);
					return;
				default:
					if (level != LogLevel.Info)
					{
						if (level != LogLevel.Debug)
						{
							return;
						}
						Counters.LogDebug.Inc(message);
						return;
					}
					else
					{
						Counters.LogMessages.Inc(message);
					}
					break;
				}
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00022F17 File Offset: 0x00021117
		public EnabledLoggingLevel EnabledLevel
		{
			get
			{
				return EnabledLoggingLevel.UpToInfo;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00022F1B File Offset: 0x0002111B
		public string Name
		{
			get
			{
				return "Instrumentation logger";
			}
		}
	}
}
