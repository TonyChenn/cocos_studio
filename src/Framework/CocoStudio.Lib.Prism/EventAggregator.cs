using System;
using System.Collections.Generic;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000014 RID: 20
	public class EventAggregator : IEventAggregator
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002C3C File Offset: 0x00000E3C
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002C9C File Offset: 0x00000E9C
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002CB2 File Offset: 0x00000EB2
		public static EventAggregator Instance { get; private set; } = new EventAggregator();

		// Token: 0x06000039 RID: 57 RVA: 0x00002CBA File Offset: 0x00000EBA
		private EventAggregator()
		{
		}

		// Token: 0x04000018 RID: 24
		private readonly Dictionary<Type, EventBase> events = new Dictionary<Type, EventBase>();
	}
}
