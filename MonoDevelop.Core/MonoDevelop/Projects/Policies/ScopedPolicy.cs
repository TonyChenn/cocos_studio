using System;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001F3 RID: 499
	public class ScopedPolicy<T>
	{
		// Token: 0x06001308 RID: 4872 RVA: 0x0004ED78 File Offset: 0x0004CF78
		public ScopedPolicy(T policy, string scope)
		{
			this.Policy = policy;
			this.Scope = scope;
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x0004ED8E File Offset: 0x0004CF8E
		// (set) Token: 0x0600130A RID: 4874 RVA: 0x0004ED96 File Offset: 0x0004CF96
		public string Scope { get; internal set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x0004ED9F File Offset: 0x0004CF9F
		// (set) Token: 0x0600130C RID: 4876 RVA: 0x0004EDA7 File Offset: 0x0004CFA7
		public T Policy { get; internal set; }

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x0004EDB0 File Offset: 0x0004CFB0
		// (set) Token: 0x0600130E RID: 4878 RVA: 0x0004EDB8 File Offset: 0x0004CFB8
		internal bool SupportsDiffSerialize { get; set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x0004EDC1 File Offset: 0x0004CFC1
		public virtual Type PolicyType
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
