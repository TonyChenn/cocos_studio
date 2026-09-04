using System;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000013 RID: 19
	public interface IEventAggregator
	{
		// Token: 0x06000035 RID: 53
		TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
	}
}
