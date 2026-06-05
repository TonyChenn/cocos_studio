using System;

namespace CocoStudio.Core
{
	// Token: 0x0200002D RID: 45
	public interface INetworkService
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001B2 RID: 434
		bool IsOK { get; }

		// Token: 0x060001B3 RID: 435
		void Intinalize(string requestUrl, int? interval = null);

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001B4 RID: 436
		// (remove) Token: 0x060001B5 RID: 437
		event EventHandler<NetworkChangedEventArgs> NetworkChanged;

		// Token: 0x060001B6 RID: 438
		void TryRequest();
	}
}
