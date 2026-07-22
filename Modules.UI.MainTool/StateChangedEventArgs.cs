using System;

namespace Modules.UI.MainTool
{
	// Token: 0x0200000F RID: 15
	public class StateChangedEventArgs : EventArgs
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003A18 File Offset: 0x00001C18
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003A2F File Offset: 0x00001C2F
		public string State { get; private set; }

		// Token: 0x06000059 RID: 89 RVA: 0x00003A38 File Offset: 0x00001C38
		public StateChangedEventArgs(string state)
		{
			this.State = (state ?? string.Empty);
		}
	}
}
