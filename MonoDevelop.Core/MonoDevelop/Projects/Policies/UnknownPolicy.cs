using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001EC RID: 492
	internal class UnknownPolicy : IEquatable<UnknownPolicy>
	{
		// Token: 0x060012DB RID: 4827 RVA: 0x0004E6E0 File Offset: 0x0004C8E0
		public UnknownPolicy(DataNode data)
		{
			this.Data = data;
			this.scope = "unknown:" + ++UnknownPolicy.upCount;
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0004E711 File Offset: 0x0004C911
		public bool Equals(UnknownPolicy other)
		{
			return false;
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0004E714 File Offset: 0x0004C914
		public string Scope
		{
			get
			{
				return this.scope;
			}
		}

		// Token: 0x04000580 RID: 1408
		private static int upCount;

		// Token: 0x04000581 RID: 1409
		private string scope;

		// Token: 0x04000582 RID: 1410
		public DataNode Data;
	}
}
