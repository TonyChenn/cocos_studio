using System;
using System.Collections.Generic;
using Mono.Addins;

namespace MonoDevelop.Core.LogReporting
{
	// Token: 0x0200026C RID: 620
	[TypeExtensionPoint]
	public abstract class CrashReporter
	{
		// Token: 0x06001664 RID: 5732
		public abstract void ReportCrash(Exception ex, bool willShutDown, IEnumerable<string> tags);
	}
}
