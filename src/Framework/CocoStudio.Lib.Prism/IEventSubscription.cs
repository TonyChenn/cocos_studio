using System;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000008 RID: 8
	public interface IEventSubscription
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11
		// (set) Token: 0x0600000C RID: 12
		SubscriptionToken SubscriptionToken { get; set; }

		// Token: 0x0600000D RID: 13
		Action<object[]> GetExecutionStrategy();
	}
}
