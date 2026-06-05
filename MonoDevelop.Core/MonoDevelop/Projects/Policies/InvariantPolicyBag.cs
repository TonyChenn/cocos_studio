using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001ED RID: 493
	internal class InvariantPolicyBag : PolicyContainer
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x0004E71E File Offset: 0x0004C91E
		public override bool IsRoot
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0004E721 File Offset: 0x0004C921
		public override PolicyContainer ParentPolicies
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0004E724 File Offset: 0x0004C924
		protected override T GetDefaultPolicy<T>()
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0004E72B File Offset: 0x0004C92B
		protected override T GetDefaultPolicy<T>(IEnumerable<string> scopes)
		{
			return Activator.CreateInstance<T>();
		}
	}
}
