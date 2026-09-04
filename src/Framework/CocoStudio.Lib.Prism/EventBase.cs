using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x0200000B RID: 11
	public abstract class EventBase
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002424 File Offset: 0x00000624
		protected ICollection<IEventSubscription> Subscriptions
		{
			get
			{
				return this._subscriptions;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000243C File Offset: 0x0000063C
		protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
		{
			if (eventSubscription == null)
			{
				throw new ArgumentNullException("eventSubscription");
			}
			eventSubscription.SubscriptionToken = new SubscriptionToken(new Action<SubscriptionToken>(this.Unsubscribe));
			lock (this.Subscriptions)
			{
				this.Subscriptions.Add(eventSubscription);
			}
			return eventSubscription.SubscriptionToken;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000024C8 File Offset: 0x000006C8
		protected virtual void InternalPublish(params object[] arguments)
		{
			List<Action<object[]>> list = this.PruneAndReturnStrategies();
			foreach (Action<object[]> action in list)
			{
				action(arguments);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002550 File Offset: 0x00000750
		public virtual void Unsubscribe(SubscriptionToken token)
		{
			lock (this.Subscriptions)
			{
				IEventSubscription eventSubscription = this.Subscriptions.FirstOrDefault((IEventSubscription evt) => evt.SubscriptionToken == token);
				if (eventSubscription != null)
				{
					this.Subscriptions.Remove(eventSubscription);
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000260C File Offset: 0x0000080C
		public virtual bool Contains(SubscriptionToken token)
		{
			bool result;
			lock (this.Subscriptions)
			{
				IEventSubscription eventSubscription = this.Subscriptions.FirstOrDefault((IEventSubscription evt) => evt.SubscriptionToken == token);
				result = (eventSubscription != null);
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002690 File Offset: 0x00000890
		private List<Action<object[]>> PruneAndReturnStrategies()
		{
			List<Action<object[]>> list = new List<Action<object[]>>();
			lock (this.Subscriptions)
			{
				for (int i = this.Subscriptions.Count - 1; i >= 0; i--)
				{
					Action<object[]> executionStrategy = this._subscriptions[i].GetExecutionStrategy();
					if (executionStrategy == null)
					{
						this._subscriptions.RemoveAt(i);
					}
					else
					{
						list.Add(executionStrategy);
					}
				}
			}
			return list;
		}

		// Token: 0x0400000F RID: 15
		private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();
	}
}
