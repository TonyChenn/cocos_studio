using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class AddinDependencyAttribute : Attribute
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000033B8 File Offset: 0x000015B8
		public AddinDependencyAttribute(string addinId)
		{
			this.addin = addinId;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000033C7 File Offset: 0x000015C7
		public string Addin
		{
			get
			{
				return this.addin;
			}
		}

		// Token: 0x04000030 RID: 48
		private readonly string addin;
	}
}
