using System;
using System.ComponentModel;

namespace MonoDevelop.Core
{
	// Token: 0x02000097 RID: 151
	public class LocalizedDisplayNameAttribute : DisplayNameAttribute
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x00011157 File Offset: 0x0000F357
		public LocalizedDisplayNameAttribute(string displayName) : base(displayName)
		{
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00011160 File Offset: 0x0000F360
		public override string DisplayName
		{
			get
			{
				return GettextCatalog.GetString(base.DisplayName);
			}
		}
	}
}
