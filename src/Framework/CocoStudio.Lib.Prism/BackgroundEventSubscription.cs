using System;
using System.Threading;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x0200000A RID: 10
	public class BackgroundEventSubscription<TPayload> : EventSubscription<TPayload>
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000023C0 File Offset: 0x000005C0
		public BackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference) : base(actionReference, filterReference)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023EC File Offset: 0x000005EC
		public override void InvokeAction(Action<TPayload> action, TPayload argument)
		{
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				action(argument);
			});
		}
	}
}
