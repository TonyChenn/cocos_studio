using System;
using System.Globalization;

namespace CocoStudio.Lib.Prism
{
	public class EventSubscription<TPayload> : IEventSubscription
	{
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

		public Action<TPayload> Action
		{
			get
			{
				return (Action<TPayload>)this._actionReference.Target;
			}
		}

		public Predicate<TPayload> Filter
		{
			get
			{
				return (Predicate<TPayload>)this._filterReference.Target;
			}
		}

		public SubscriptionToken SubscriptionToken { get; set; }

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

		public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			action(argument);
		}

		private readonly IDelegateReference _actionReference;

		private readonly IDelegateReference _filterReference;
	}
}
