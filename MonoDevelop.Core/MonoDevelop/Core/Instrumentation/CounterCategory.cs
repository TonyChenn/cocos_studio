using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000D3 RID: 211
	[Serializable]
	public class CounterCategory
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x0001C83C File Offset: 0x0001AA3C
		internal CounterCategory(string name)
		{
			this.name = name;
			this.counters = new List<Counter>();
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0001C856 File Offset: 0x0001AA56
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001C85E File Offset: 0x0001AA5E
		internal void AddCounter(Counter c)
		{
			this.counters.Add(c);
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0001C86C File Offset: 0x0001AA6C
		public IEnumerable<Counter> Counters
		{
			get
			{
				return this.counters;
			}
		}

		// Token: 0x0400026A RID: 618
		private string name;

		// Token: 0x0400026B RID: 619
		private List<Counter> counters;
	}
}
