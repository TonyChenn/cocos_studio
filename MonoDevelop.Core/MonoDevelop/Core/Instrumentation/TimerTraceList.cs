using System;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000E1 RID: 225
	[Serializable]
	internal class TimerTraceList
	{
		// Token: 0x0400028A RID: 650
		public TimerTrace FirstTrace;

		// Token: 0x0400028B RID: 651
		public TimeSpan TotalTime;

		// Token: 0x0400028C RID: 652
		public int ValueIndex;
	}
}
