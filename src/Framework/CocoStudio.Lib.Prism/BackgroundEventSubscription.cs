using System;
using System.Threading;

namespace CocoStudio.Lib.Prism
{
	public class BackgroundEventSubscription<TPayload> : EventSubscription<TPayload>
	{
		public BackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference) : base(actionReference, filterReference)
		{
		}

		public override void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				action(argument);
			});
		}
	}
}
