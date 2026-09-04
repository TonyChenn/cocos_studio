using System;

namespace CocoStudio.ControlLib
{
	// Token: 0x02000005 RID: 5
	public class DialogResult
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002788 File Offset: 0x00000988
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000279F File Offset: 0x0000099F
		public bool IsChangedAll { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000027A8 File Offset: 0x000009A8
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000027BF File Offset: 0x000009BF
		public EFileOperate ButtonResult { get; set; }
	}
}
