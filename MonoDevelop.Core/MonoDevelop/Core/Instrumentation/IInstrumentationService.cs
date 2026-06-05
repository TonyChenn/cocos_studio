using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Instrumentation
{
	// Token: 0x020000CD RID: 205
	public interface IInstrumentationService
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060006FB RID: 1787
		DateTime StartTime { get; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006FC RID: 1788
		DateTime EndTime { get; }

		// Token: 0x060006FD RID: 1789
		IEnumerable<Counter> GetCounters();

		// Token: 0x060006FE RID: 1790
		Counter GetCounter(string name);

		// Token: 0x060006FF RID: 1791
		CounterCategory GetCategory(string name);

		// Token: 0x06000700 RID: 1792
		IEnumerable<CounterCategory> GetCategories();
	}
}
