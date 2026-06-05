using System;
using Mono.Addins;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x02000096 RID: 150
	internal class PackageNotInstalledCondition : PackageInstalledCondition
	{
		// Token: 0x060004EC RID: 1260 RVA: 0x00011143 File Offset: 0x0000F343
		public override bool Evaluate(NodeElement conditionNode)
		{
			return !base.Evaluate(conditionNode);
		}
	}
}
