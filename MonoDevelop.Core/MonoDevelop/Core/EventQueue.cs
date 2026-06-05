using System;
using System.Collections.Generic;

namespace MonoDevelop.Core
{
	// Token: 0x02000036 RID: 54
	internal class EventQueue
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x00008501 File Offset: 0x00006701
		public EventQueue()
		{
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00008514 File Offset: 0x00006714
		public EventQueue(object defaultSourceObject)
		{
			this.defaultSourceObject = defaultSourceObject;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008530 File Offset: 0x00006730
		public void Freeze()
		{
			lock (this.events)
			{
				this.frozen++;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008578 File Offset: 0x00006778
		public void Thaw()
		{
			List<EventQueue.EventData> list = null;
			lock (this.events)
			{
				if (--this.frozen == 0)
				{
					list = this.events;
					this.events = new List<EventQueue.EventData>();
				}
			}
			if (list != null)
			{
				int i = 0;
				while (i < list.Count)
				{
					EventQueue.EventData eventData = list[i];
					Delegate @delegate = eventData.Delegate();
					if (!(eventData.Args is IEventArgsChain))
					{
						goto IL_F4;
					}
					EventQueue.EventData eventData2 = (i < list.Count - 1) ? list[i + 1] : null;
					if (eventData2 == null || !(eventData2.Args.GetType() == eventData.Args.GetType()) || !(eventData2.Delegate() == @delegate) || eventData2.ThisObject != eventData.ThisObject)
					{
						goto IL_F4;
					}
					((IEventArgsChain)eventData2.Args).MergeWith((IEventArgsChain)eventData.Args);
					IL_11C:
					i++;
					continue;
					IL_F4:
					if (@delegate != null)
					{
						@delegate.DynamicInvoke(new object[]
						{
							eventData.ThisObject,
							eventData.Args
						});
						goto IL_11C;
					}
					goto IL_11C;
				}
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000086C4 File Offset: 0x000068C4
		public void RaiseEvent(Func<Delegate> d, EventArgs args)
		{
			this.RaiseEvent(d, this.defaultSourceObject, args);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00008710 File Offset: 0x00006910
		public void RaiseEvent(Func<Delegate> d, object thisObj, EventArgs args)
		{
			Delegate del = d();
			lock (this.events)
			{
				if (this.frozen > 0)
				{
					EventQueue.EventData eventData = new EventQueue.EventData();
					eventData.Delegate = d;
					eventData.ThisObject = thisObj;
					eventData.Args = args;
					this.events.Add(eventData);
					return;
				}
			}
			if (del != null)
			{
				Runtime.MainSynchronizationContext.Post(delegate(object param0)
				{
					del.DynamicInvoke(new object[]
					{
						thisObj,
						args
					});
				}, null);
			}
		}

		// Token: 0x040000B5 RID: 181
		private List<EventQueue.EventData> events = new List<EventQueue.EventData>();

		// Token: 0x040000B6 RID: 182
		private int frozen;

		// Token: 0x040000B7 RID: 183
		private object defaultSourceObject;

		// Token: 0x02000037 RID: 55
		private class EventData
		{
			// Token: 0x040000B8 RID: 184
			public Func<Delegate> Delegate;

			// Token: 0x040000B9 RID: 185
			public object ThisObject;

			// Token: 0x040000BA RID: 186
			public EventArgs Args;
		}
	}
}
