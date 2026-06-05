using System;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001F5 RID: 501
	internal struct PolicyKey : IEquatable<PolicyKey>
	{
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001313 RID: 4883 RVA: 0x0004EDFF File Offset: 0x0004CFFF
		// (set) Token: 0x06001314 RID: 4884 RVA: 0x0004EE07 File Offset: 0x0004D007
		public Type PolicyType { get; private set; }

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x0004EE10 File Offset: 0x0004D010
		// (set) Token: 0x06001316 RID: 4886 RVA: 0x0004EE18 File Offset: 0x0004D018
		public string Scope { get; private set; }

		// Token: 0x06001317 RID: 4887 RVA: 0x0004EE21 File Offset: 0x0004D021
		public PolicyKey(Type policyType, string scope)
		{
			this = default(PolicyKey);
			this.PolicyType = policyType;
			this.Scope = scope;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0004EE38 File Offset: 0x0004D038
		public override bool Equals(object obj)
		{
			return obj is PolicyKey && this.Equals((PolicyKey)obj);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0004EE50 File Offset: 0x0004D050
		public bool Equals(PolicyKey other)
		{
			return other.PolicyType.AssemblyQualifiedName == this.PolicyType.AssemblyQualifiedName && other.Scope == this.Scope;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0004EE84 File Offset: 0x0004D084
		public override int GetHashCode()
		{
			int num = this.PolicyType.AssemblyQualifiedName.GetHashCode();
			if (this.Scope != null)
			{
				num += this.Scope.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0004EEB9 File Offset: 0x0004D0B9
		public override string ToString()
		{
			if (this.Scope != null)
			{
				return string.Format("[Policy: Type={0}, scope={1}]", this.PolicyType, this.Scope);
			}
			return string.Format("[Policy: Type={0}]", this.PolicyType);
		}
	}
}
