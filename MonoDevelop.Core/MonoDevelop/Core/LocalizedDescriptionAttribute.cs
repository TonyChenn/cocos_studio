using System;
using System.ComponentModel;

namespace MonoDevelop.Core
{
	// Token: 0x02000098 RID: 152
	public class LocalizedDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x0001116D File Offset: 0x0000F36D
		public LocalizedDescriptionAttribute(string description) : base(description)
		{
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00011176 File Offset: 0x0000F376
		public override string Description
		{
			get
			{
				return GettextCatalog.GetString(base.Description);
			}
		}
	}
}
