using System;

namespace CocoStudio.Lib.Prism
{
	public interface IEventSubscription
	{
		SubscriptionToken SubscriptionToken { get; set; }

		Action<object[]> GetExecutionStrategy();
	}
}
