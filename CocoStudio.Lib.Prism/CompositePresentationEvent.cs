using System;
using System.Linq;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x0200000C RID: 12
	public class CompositePresentationEvent<TPayload> : EventBase
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002758 File Offset: 0x00000958
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

		// Token: 0x0600001F RID: 31 RVA: 0x00002790 File Offset: 0x00000990
		public SubscriptionToken Subscribe(Action<TPayload> action)
		{
			return this.Subscribe(action, ThreadOption.PublisherThread);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027AC File Offset: 0x000009AC
		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption)
		{
			return this.Subscribe(action, threadOption, false);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027C8 File Offset: 0x000009C8
		public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
		{
			return this.Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027E4 File Offset: 0x000009E4
		public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
		{
			return this.Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002814 File Offset: 0x00000A14
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

		// Token: 0x06000024 RID: 36 RVA: 0x000028BC File Offset: 0x00000ABC
		public virtual void Publish(TPayload payload)
		{
			base.InternalPublish(new object[]
			{
				payload
			});
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002910 File Offset: 0x00000B10
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

		// Token: 0x06000026 RID: 38 RVA: 0x000029D4 File Offset: 0x00000BD4
		public virtual bool Contains(Action<TPayload> subscriber)
		{
			IEventSubscription eventSubscription;
			lock (base.Subscriptions)
			{
				eventSubscription = base.Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault((EventSubscription<TPayload> evt) => evt.Action == subscriber);
			}
			return eventSubscription != null;
		}

		// Token: 0x04000010 RID: 16
		private IDispatcherFacade uiDispatcher;
	}
}
