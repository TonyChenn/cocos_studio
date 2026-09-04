using System;

namespace CocoStudio.Core
{
	// Token: 0x02000030 RID: 48
	public class NetworkChangedEventArgs : EventArgs
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00008AA4 File Offset: 0x00006CA4
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00008ABB File Offset: 0x00006CBB
		public bool IsNetworkingSuccessed { get; private set; }

		// Token: 0x060001CB RID: 459 RVA: 0x00008AC4 File Offset: 0x00006CC4
		public NetworkChangedEventArgs(bool isNetworkingSuccessed)
		{
			this.IsNetworkingSuccessed = isNetworkingSuccessed;
		}
	}
}
