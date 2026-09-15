using System;

namespace CocoStudio.Lib.Prism
{
	public interface IEventAggregator
	{
		TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
	}
}
