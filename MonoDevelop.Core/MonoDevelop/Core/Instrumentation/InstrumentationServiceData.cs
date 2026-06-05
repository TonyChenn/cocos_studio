using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000CF RID: 207
	[Serializable]
	internal class InstrumentationServiceData : IInstrumentationService
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0001BC65 File Offset: 0x00019E65
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0001BC6D File Offset: 0x00019E6D
		public DateTime StartTime { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001BC76 File Offset: 0x00019E76
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0001BC7E File Offset: 0x00019E7E
		public DateTime EndTime { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001BC87 File Offset: 0x00019E87
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001BC8F File Offset: 0x00019E8F
		public Dictionary<string, Counter> Counters { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001BC98 File Offset: 0x00019E98
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001BCA0 File Offset: 0x00019EA0
		public List<CounterCategory> Categories { get; set; }

		// Token: 0x06000711 RID: 1809 RVA: 0x0001BCA9 File Offset: 0x00019EA9
		public IEnumerable<Counter> GetCounters()
		{
			return this.Counters.Values;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001BCB8 File Offset: 0x00019EB8
		public Counter GetCounter(string name)
		{
			Counter counter;
			if (this.Counters.TryGetValue(name, out counter))
			{
				return counter;
			}
			counter = new Counter(name, null);
			this.Counters[name] = counter;
			return counter;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001BD08 File Offset: 0x00019F08
		public CounterCategory GetCategory(string name)
		{
			return this.Categories.FirstOrDefault((CounterCategory c) => c.Name == name);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001BD39 File Offset: 0x00019F39
		public IEnumerable<CounterCategory> GetCategories()
		{
			return this.Categories;
		}
	}
}
