using System;
using System.Linq;

namespace CocoStudio.Lib.Prism
{
	public class CompositePresentationEvent<TPayload> : EventBase
	{
		private IDispatcherFacade UIDispatcher
		{
			get
			{
				if (this.uiDispatcher == null)
				{
					this.uiDispatcher = new DefaultDispatcher();
				}
				return this.uiDispatcher;
			}
		}

		public SubscriptionToken Subscribe(Action<TPayload> action)
		{
			return this.Subscribe(action, ThreadOption.PublisherThread);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption)
		{
			return this.Subscribe(action, threadOption, false);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
		{
			return this.Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
		}

		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
		{
			return this.Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
		}

		public virtual SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
		{
			IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
			IDelegateReference filterReference;
			if (filter != null)
			{
				filterReference = new DelegateReference(filter, keepSubscriberReferenceAlive);
			}
			else
			{
				filterReference = new DelegateReference(new Predicate<TPayload>((TPayload param0) => true), true);
			}
			EventSubscription<TPayload> eventSubscription;
			switch (threadOption)
			{
			case ThreadOption.PublisherThread:
				eventSubscription = new EventSubscription<TPayload>(actionReference, filterReference);
				break;
			case ThreadOption.UIThread:
				eventSubscription = new DispatcherEventSubscription<TPayload>(actionReference, filterReference, this.UIDispatcher);
				break;
			case ThreadOption.BackgroundThread:
				eventSubscription = new BackgroundEventSubscription<TPayload>(actionReference, filterReference);
				break;
			default:
				eventSubscription = new EventSubscription<TPayload>(actionReference, filterReference);
				break;
			}
			return base.InternalSubscribe(eventSubscription);
		}

		public virtual void Publish(TPayload payload)
		{
			base.InternalPublish(new object[]
			{
				payload
			});
		}

		public virtual void Unsubscribe(Action<TPayload> subscriber)
		{
			lock (base.Subscriptions)
			{
				IEventSubscription eventSubscription = base.Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault((EventSubscription<TPayload> evt) => evt.Action == subscriber);
				if (eventSubscription != null)
				{
					base.Subscriptions.Remove(eventSubscription);
				}
			}
		}

		public virtual bool Contains(Action<TPayload> subscriber)
		{
			IEventSubscription eventSubscription;
			lock (base.Subscriptions)
			{
				eventSubscription = base.Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault((EventSubscription<TPayload> evt) => evt.Action == subscriber);
			}
			return eventSubscription != null;
		}

		private IDispatcherFacade uiDispatcher;
	}
}
