using System;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000004 RID: 4
	public class ProgressChangedArgs : EventArgs
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024C7 File Offset: 0x000006C7
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000024CF File Offset: 0x000006CF
		public float Progress { get; private set; }

		// Token: 0x06000024 RID: 36 RVA: 0x000024D8 File Offset: 0x000006D8
		public ProgressChangedArgs(float progress)
		{
			this.Progress = progress;
		}
	}
}
