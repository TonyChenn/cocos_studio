using System;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000012 RID: 18
	public class DispatcherEventSubscription<TPayload> : EventSubscription<TPayload>
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002C11 File Offset: 0x00000E11
		public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, IDispatcherFacade dispatcher) : base(actionReference, filterReference)
		{
			this.dispatcher = dispatcher;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002C25 File Offset: 0x00000E25
		public override void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			this.dispatcher.BeginInvoke(action, argument);
		}

		// Token: 0x04000017 RID: 23
		private readonly IDispatcherFacade dispatcher;
	}
}
