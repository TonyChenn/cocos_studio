using System;
using System.Collections.Generic;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000013 RID: 19
	public abstract class CompositeFormat : FileFormat, ICompositeResourceProcesser
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002CE1 File Offset: 0x00000EE1
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002CE9 File Offset: 0x00000EE9
		public bool IsHiddenCompositeFile { get; set; }

		// Token: 0x06000059 RID: 89 RVA: 0x00002CF2 File Offset: 0x00000EF2
		public CompositeFormat()
		{
			this.IsHiddenCompositeFile = true;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002D01 File Offset: 0x00000F01
		public virtual bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002D19 File Offset: 0x00000F19
		public virtual List<string> GetFiles(string filePath)
		{
			return null;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002D1C File Offset: 0x00000F1C
		public virtual List<string> GetPretreatmentTypes()
		{
			return null;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002D1F File Offset: 0x00000F1F
		public virtual List<string> GetFilterTypes()
		{
			return null;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002D22 File Offset: 0x00000F22
		public virtual List<string> GetAfterTypes()
		{
			return null;
		}
	}
}
