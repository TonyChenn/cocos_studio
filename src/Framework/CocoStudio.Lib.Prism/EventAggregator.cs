using System;
using System.Collections.Generic;

namespace CocoStudio.Lib.Prism
{
	public class EventAggregator : IEventAggregator
	{
		public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
		{
			EventBase eventBase = null;
			TEventType result;
			if (!this.events.TryGetValue(typeof(TEventType), out eventBase))
			{
				TEventType teventType = Activator.CreateInstance<TEventType>();
				this.events[typeof(TEventType)] = teventType;
				result = teventType;
			}
			else
			{
				result = (TEventType)((object)eventBase);
			}
			return result;
		}

		public static EventAggregator Instance { get; private set; } = new EventAggregator();

		private EventAggregator()
		{
		}

		private readonly Dictionary<Type, EventBase> events = new Dictionary<Type, EventBase>();
	}
}
