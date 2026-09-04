using System;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000011 RID: 17
	public class CurrentToolChangedEventArgs : EventArgs
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005F90 File Offset: 0x00004190
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00005FA7 File Offset: 0x000041A7
		public ITool Current { get; private set; }

		// Token: 0x060000AC RID: 172 RVA: 0x00005FB0 File Offset: 0x000041B0
		public CurrentToolChangedEventArgs(ITool currentTool)
		{
			this.Current = currentTool;
		}
	}
}
