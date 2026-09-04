using System;
using System.Globalization;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000009 RID: 9
	public class EventSubscription<TPayload> : IEventSubscription
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000217C File Offset: 0x0000037C
		public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
		{
			if (actionReference == null)
			{
				throw new ArgumentNullException("actionReference");
			}
			if (!(actionReference.Target is Action<TPayload>))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The Target of the IDelegateReference should be of type {0}.", new object[]
				{
					typeof(Action<TPayload>).FullName
				}), "actionReference");
			}
			if (filterReference == null)
			{
				throw new ArgumentNullException("filterReference");
			}
			if (!(filterReference.Target is Predicate<TPayload>))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The Target of the IDelegateReference should be of type {0}.", new object[]
				{
					typeof(Predicate<TPayload>).FullName
				}), "filterReference");
			}
			this._actionReference = actionReference;
			this._filterReference = filterReference;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002258 File Offset: 0x00000458
		public Action<TPayload> Action
		{
			get
			{
				return (Action<TPayload>)this._actionReference.Target;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000227C File Offset: 0x0000047C
		public Predicate<TPayload> Filter
		{
			get
			{
				return (Predicate<TPayload>)this._filterReference.Target;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022A0 File Offset: 0x000004A0
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000022B7 File Offset: 0x000004B7
		public SubscriptionToken SubscriptionToken { get; set; }

		// Token: 0x06000013 RID: 19 RVA: 0x00002328 File Offset: 0x00000528
		public virtual Action<object[]> GetExecutionStrategy()
		{
			Action<TPayload> action = this.Action;
			Predicate<TPayload> filter = this.Filter;
			Action<object[]> result;
			if (action != null && filter != null)
			{
				result = delegate(object[] arguments)
				{
					TPayload tpayload = default(TPayload);
					if (arguments != null && arguments.Length > 0 && arguments[0] != null)
					{
						tpayload = (TPayload)((object)arguments[0]);
					}
					if (filter(tpayload))
					{
						this.InvokeAction(action, tpayload);
					}
				};
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002394 File Offset: 0x00000594
		public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			action(argument);
		}

		// Token: 0x0400000C RID: 12
		private readonly IDelegateReference _actionReference;

		// Token: 0x0400000D RID: 13
		private readonly IDelegateReference _filterReference;
	}
}
