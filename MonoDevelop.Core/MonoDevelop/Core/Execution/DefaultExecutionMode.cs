using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000C4 RID: 196
	internal class DefaultExecutionMode : IExecutionMode
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001AAE9 File Offset: 0x00018CE9
		public string Name
		{
			get
			{
				return GettextCatalog.GetString("Default");
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0001AAF5 File Offset: 0x00018CF5
		public string Id
		{
			get
			{
				return "Default";
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001AAFC File Offset: 0x00018CFC
		public IExecutionHandler ExecutionHandler
		{
			get
			{
				return Runtime.ProcessService.DefaultExecutionHandler;
			}
		}
	}
}
