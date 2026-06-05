using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200017C RID: 380
	public class UnknownProjectItem : ProjectItem
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00038DE5 File Offset: 0x00036FE5
		// (set) Token: 0x06000F1E RID: 3870 RVA: 0x00038DED File Offset: 0x00036FED
		public string ItemName { get; private set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00038DF6 File Offset: 0x00036FF6
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x00038DFE File Offset: 0x00036FFE
		[ItemProperty("Include")]
		public string Include { get; private set; }

		// Token: 0x06000F21 RID: 3873 RVA: 0x00038E07 File Offset: 0x00037007
		public UnknownProjectItem(string name, string include)
		{
			this.ItemName = name;
			this.Include = include;
		}
	}
}
