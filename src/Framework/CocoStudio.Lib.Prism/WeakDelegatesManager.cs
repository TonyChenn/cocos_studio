using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Lib.Prism
{
	internal class WeakDelegatesManager
	{
		public void AddListener(Delegate listener)
		{
			this.listeners.Add(new DelegateReference(listener, false));
		}

		public void RemoveListener(Delegate listener)
		{
			this.listeners.RemoveAll(delegate(DelegateReference reference)
			{
				Delegate target = reference.Target;
				return listener.Equals(target) || target == null;
			});
		}

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

		private readonly List<DelegateReference> listeners = new List<DelegateReference>();
	}
}
