using System;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x020000DD RID: 221
	public class RemoteLogger : MarshalByRefObject, ILogger
	{
		// Token: 0x060007DE RID: 2014 RVA: 0x00020569 File Offset: 0x0001E769
		public void Log(LogLevel level, string message)
		{
			LoggingService.Log(level, message);
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00020572 File Offset: 0x0001E772
		public EnabledLoggingLevel EnabledLevel
		{
			get
			{
				return EnabledLoggingLevel.UpToDebug;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00020576 File Offset: 0x0001E776
		public string Name
		{
			get
			{
				return "Main Process Logger";
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002057D File Offset: 0x0001E77D
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
