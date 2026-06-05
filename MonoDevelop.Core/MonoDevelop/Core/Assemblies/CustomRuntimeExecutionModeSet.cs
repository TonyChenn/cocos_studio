using System;
using System.Collections.Generic;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000AC RID: 172
	public class CustomRuntimeExecutionModeSet : IExecutionModeSet
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x000164E5 File Offset: 0x000146E5
		public string Name
		{
			get
			{
				return "Custom Runtime";
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00016698 File Offset: 0x00014898
		public IEnumerable<IExecutionMode> ExecutionModes
		{
			get
			{
				foreach (TargetRuntime tr in Runtime.SystemAssemblyService.GetTargetRuntimes())
				{
					yield return new ExecutionMode(tr.Id, tr.DisplayName, tr.GetExecutionHandler());
				}
				yield break;
			}
		}
	}
}
