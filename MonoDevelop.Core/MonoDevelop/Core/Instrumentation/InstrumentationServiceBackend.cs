using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000CE RID: 206
	internal class InstrumentationServiceBackend : MarshalByRefObject, IInstrumentationService
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001BC2E File Offset: 0x00019E2E
		public DateTime StartTime
		{
			get
			{
				return InstrumentationService.StartTime;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001BC35 File Offset: 0x00019E35
		public DateTime EndTime
		{
			get
			{
				return DateTime.Now;
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001BC3C File Offset: 0x00019E3C
		public IEnumerable<Counter> GetCounters()
		{
			return InstrumentationService.GetCounters();
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001BC43 File Offset: 0x00019E43
		public Counter GetCounter(string name)
		{
			return InstrumentationService.GetCounter(name);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001BC4B File Offset: 0x00019E4B
		public CounterCategory GetCategory(string name)
		{
			return InstrumentationService.GetCategory(name);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001BC53 File Offset: 0x00019E53
		public IEnumerable<CounterCategory> GetCategories()
		{
			return InstrumentationService.GetCategories();
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001BC5A File Offset: 0x00019E5A
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
