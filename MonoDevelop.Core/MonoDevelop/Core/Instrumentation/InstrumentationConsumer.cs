using System;
using Mono.Addins;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x0200026D RID: 621
	[TypeExtensionPoint]
	public abstract class InstrumentationConsumer
	{
		// Token: 0x06001666 RID: 5734
		public abstract bool SupportsCounter(Counter counter);

		// Token: 0x06001667 RID: 5735 RVA: 0x0005A58F File Offset: 0x0005878F
		public virtual void ConsumeValue(Counter counter, CounterValue value)
		{
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0005A591 File Offset: 0x00058791
		public virtual IDisposable BeginTimer(TimerCounter counter, CounterValue value)
		{
			return null;
		}
	}
}
