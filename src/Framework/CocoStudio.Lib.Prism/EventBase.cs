using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Lib.Prism
{
	public abstract class EventBase
	{
		protected ICollection<IEventSubscription> Subscriptions
		{
			get
			{
				return this._subscriptions;
			}
		}

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

		protected virtual void InternalPublish(params object[] arguments)
		{
			List<Action<object[]>> list = this.PruneAndReturnStrategies();
			foreach (Action<object[]> action in list)
			{
				action(arguments);
			}
		}

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

		private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();
	}
}
