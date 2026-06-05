using System;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001F2 RID: 498
	public class PolicyChangedEventArgs : EventArgs
	{
		// Token: 0x06001303 RID: 4867 RVA: 0x0004ED40 File Offset: 0x0004CF40
		public PolicyChangedEventArgs(Type policyType, string scope)
		{
			this.PolicyType = policyType;
			this.Scope = scope;
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x0004ED56 File Offset: 0x0004CF56
		// (set) Token: 0x06001305 RID: 4869 RVA: 0x0004ED5E File Offset: 0x0004CF5E
		public Type PolicyType { get; private set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x0004ED67 File Offset: 0x0004CF67
		// (set) Token: 0x06001307 RID: 4871 RVA: 0x0004ED6F File Offset: 0x0004CF6F
		public string Scope { get; private set; }
	}
}
