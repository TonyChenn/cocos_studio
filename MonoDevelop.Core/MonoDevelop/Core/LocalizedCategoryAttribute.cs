using System;
using System.ComponentModel;

namespace MonoDevelop.Core
{
	// Token: 0x02000099 RID: 153
	public class LocalizedCategoryAttribute : CategoryAttribute
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x00011183 File Offset: 0x0000F383
		public LocalizedCategoryAttribute(string category) : base(category)
		{
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001118C File Offset: 0x0000F38C
		protected override string GetLocalizedString(string value)
		{
			return GettextCatalog.GetString(value);
		}
	}
}
