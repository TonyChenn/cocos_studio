using System;

namespace CocoStudio.Projects
{
	// Token: 0x02000022 RID: 34
	public class DataError
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000040C0 File Offset: 0x000022C0
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000040C8 File Offset: 0x000022C8
		public string Message { get; private set; }

		// Token: 0x060000C0 RID: 192 RVA: 0x000040D1 File Offset: 0x000022D1
		public DataError(string message = "")
		{
			this.Message = message;
		}
	}
}
