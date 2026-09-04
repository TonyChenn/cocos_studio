using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000017 RID: 23
	internal class WeakDelegatesManager
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public void AddListener(Delegate listener)
		{
			this.listeners.Add(new DelegateReference(listener, false));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002E18 File Offset: 0x00001018
		public void RemoveListener(Delegate listener)
		{
			this.listeners.RemoveAll(delegate(DelegateReference reference)
			{
				Delegate target = reference.Target;
				return listener.Equals(target) || target == null;
			});
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002E98 File Offset: 0x00001098
		public void Raise(params object[] args)
		{
			this.listeners.RemoveAll((DelegateReference listener) => listener.Target == null);
			foreach (Delegate @delegate in from listener in this.listeners.ToList<DelegateReference>()
			select listener.Target into listener
			where listener != null
			select listener)
			{
				@delegate.DynamicInvoke(args);
			}
		}

		// Token: 0x04000020 RID: 32
		private readonly List<DelegateReference> listeners = new List<DelegateReference>();
	}
}
