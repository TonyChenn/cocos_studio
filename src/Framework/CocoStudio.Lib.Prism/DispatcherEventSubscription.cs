using System;

namespace CocoStudio.Lib.Prism
{
	public class DispatcherEventSubscription<TPayload> : EventSubscription<TPayload>
	{
		public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, IDispatcherFacade dispatcher) : base(actionReference, filterReference)
		{
			this.dispatcher = dispatcher;
		}

		public override void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			this.dispatcher.BeginInvoke(action, argument);
		}

		private readonly IDispatcherFacade dispatcher;
	}
}
