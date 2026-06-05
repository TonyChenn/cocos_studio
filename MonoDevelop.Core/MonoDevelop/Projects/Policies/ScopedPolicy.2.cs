using System;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001F4 RID: 500
	public class ScopedPolicy : ScopedPolicy<object>
	{
		// Token: 0x06001310 RID: 4880 RVA: 0x0004EDCD File Offset: 0x0004CFCD
		public ScopedPolicy(Type type, object ob, string scope) : base(ob, scope)
		{
			this.type = type;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0004EDDE File Offset: 0x0004CFDE
		internal ScopedPolicy(Type type, object ob, string scope, bool supportsDiffSerialize) : base(ob, scope)
		{
			this.type = type;
			base.SupportsDiffSerialize = supportsDiffSerialize;
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x0004EDF7 File Offset: 0x0004CFF7
		public override Type PolicyType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x04000599 RID: 1433
		private Type type;
	}
}
