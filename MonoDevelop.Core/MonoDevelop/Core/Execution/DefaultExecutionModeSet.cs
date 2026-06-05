using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000AB RID: 171
	internal class DefaultExecutionModeSet : IExecutionModeSet
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x000163E5 File Offset: 0x000145E5
		public string Name
		{
			get
			{
				return GettextCatalog.GetString("Default");
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x000164C0 File Offset: 0x000146C0
		public IEnumerable<IExecutionMode> ExecutionModes
		{
			get
			{
				yield return Runtime.ProcessService.DefaultExecutionMode;
				yield break;
			}
		}
	}
}
