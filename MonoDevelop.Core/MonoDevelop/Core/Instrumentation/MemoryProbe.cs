using System;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000D4 RID: 212
	public class MemoryProbe
	{
		// Token: 0x06000757 RID: 1879 RVA: 0x0001C874 File Offset: 0x0001AA74
		public MemoryProbe(Counter c)
		{
			this.c = c;
			c = ++c;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001C88C File Offset: 0x0001AA8C
		~MemoryProbe()
		{
			this.c = --this.c;
		}

		// Token: 0x0400026C RID: 620
		private Counter c;
	}
}
