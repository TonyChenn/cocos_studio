using System;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000016 RID: 22
	public class EnableChangedArgs : EventArgs
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000759F File Offset: 0x0000579F
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000075A7 File Offset: 0x000057A7
		public bool IsEnable { get; private set; }

		// Token: 0x060000A9 RID: 169 RVA: 0x000075B0 File Offset: 0x000057B0
		public EnableChangedArgs(bool isEnable)
		{
			this.IsEnable = isEnable;
		}
	}
}
